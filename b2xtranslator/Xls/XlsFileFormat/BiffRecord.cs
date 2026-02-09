
using System;
using System.Collections.Generic;
using System.Reflection;
using b2xtranslator.Spreadsheet.XlsFileFormat.Records;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.Spreadsheet.XlsFileFormat
{
    public abstract class BiffRecord
    {
        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="reader">Streamreader</param>
        /// <param name="id">Record ID - Recordtype</param>
        /// <param name="length">The recordlegth</param>
        public BiffRecord(IStreamReader reader, RecordType id, ushort length)
        {
            this.Reader = reader;
            this.Offset = this.Reader.BaseStream.Position;

            this.Id = id;
            this.Length = length;
        }

        private static readonly Dictionary<ushort, Type> typeToRecordClassMapping = new();

        static BiffRecord()
        {
            UpdateTypeToRecordClassMapping(
                Assembly.GetExecutingAssembly(),
                typeof(BOF).Namespace);
        }

        public static void UpdateTypeToRecordClassMapping(Assembly assembly, string ns)
        {
            foreach (var t in assembly.GetTypes())
            {
                if (ns == null || t.Namespace == ns)
                {
                    var attrs = t.GetCustomAttributes(typeof(BiffRecordAttribute), false);

                    BiffRecordAttribute attr = null;

                    if (attrs.Length > 0)
                        attr = attrs[0] as BiffRecordAttribute;

                    if (attr != null)
                    {
                        // Add the type codes of the array
                        foreach (ushort typeCode in attr.TypeCodes)
                        {
                            if (typeToRecordClassMapping.TryGetValue(typeCode, out var value))
                            {
                                throw new Exception($"Tried to register TypeCode {typeCode} to {t}, but it is already registered to {value}");
                            }
                            typeToRecordClassMapping.Add(typeCode, t);
                        }
                    }
                }
            }
        }


        public static RecordType GetNextRecordType(IStreamReader reader)
        {
            long position = reader.BaseStream.Position;
                
            // read type of the next record
            var nextRecord = (RecordType)reader.ReadUInt16();
            ushort length = reader.ReadUInt16();

            // skip leading StartBlock/EndBlock records
            if (nextRecord == RecordType.StartBlock
                || nextRecord == RecordType.EndBlock 
                || nextRecord == RecordType.StartObject
                || nextRecord == RecordType.EndObject
                || nextRecord == RecordType.ChartFrtInfo)
            {
                // skip the body of the record
                reader.ReadBytes(length);
                // get the type of the next record
                return GetNextRecordType(reader);
            }

            if (nextRecord == RecordType.FrtWrapper)
            {
                // return type of wrapped Biff record
                var frtWrapper = new FrtWrapper(reader, nextRecord, length);
                reader.BaseStream.Position = position;
                return frtWrapper.wrappedRecord.Id;
            }

            // seek back to the begin of the current record
            reader.BaseStream.Position = position;
            return nextRecord;
        }

        public static BiffRecord ReadRecord(IStreamReader reader)
        {
            try
            {
                var id = (RecordType)reader.ReadUInt16();
                ushort length = reader.ReadUInt16();

                // skip leading StartBlock/EndBlock records
                if (id == RecordType.StartBlock ||
                    id == RecordType.EndBlock ||
                    id == RecordType.StartObject ||
                    id == RecordType.EndObject ||
                    id == RecordType.ChartFrtInfo)
                {
                    // skip the body of this record
                    reader.ReadBytes(length);

                    // get the next record
                    return ReadRecord(reader);
                }

                if (id == RecordType.FrtWrapper)
                {
                    // return type of wrapped Biff record
                    var frtWrapper = new FrtWrapper(reader, id, length);
                    return frtWrapper.wrappedRecord;
                }

                BiffRecord result;
                if (typeToRecordClassMapping.TryGetValue((ushort)id, out var cls))
                {
                    var constructor = cls.GetConstructor(
                        [typeof(IStreamReader), typeof(RecordType), typeof(ushort)]
                    );

                    try
                    {
                        result = (BiffRecord)constructor.Invoke(
                            [reader, id, length]
                        );
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
                else
                {
                    result = new UnknownBiffRecord(reader, (RecordType)id, length);
                }

                return result;
            }
            catch (OutOfMemoryException e)
            {
                throw new Exception("Invalid BIFF record", e);
            }
        }

        public RecordType Id { get; }
        public uint Length { get; }
        public long Offset { get; }
        public IStreamReader Reader { get; set; }
    }
}
