import filesToXml, {Converter} from "./bin/filesToXml/index.mjs";
import fs from 'node:fs'
import path from 'path'

console.log(fs.readFileSync(`F://csv.csv`));
await filesToXml.boot();
const options = {
    input: [{
        path: `F://csv.csv`,
        data: fs.readFileSync(`F://csv.csv`, {encoding: 'base64'}),
        delimiter: "auto",
        codepage: 65001
    }],
    outputCodepage : 65001,
    searchingDelimiters: [';']
}
console.log(Converter.convert(options));  
await filesToXml.exit();
