namespace b2xtranslator.OfficeDrawing.Shapetypes;

/// <summary>
///     interim solution<br />
///     OOX uses an additional attribute: arcsize
/// </summary>
[OfficeShapeType(2)]
public class RoundedRectangleType : RectangleType
{
    public RoundedRectangleType()
    {
        Joins = JoinStyle.round;
        AdjustmentValues = "5400";
    }
}