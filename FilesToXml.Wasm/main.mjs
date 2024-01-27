import filesToXml, {Converter} from "./bin/filesToXml/index.mjs";
import fs from 'node:fs'
import path from 'path'

await filesToXml.boot();
// const options = {
//     input: [{
//         path: (`file://E://csv.csv`),
//         delimiter: "auto",
//         codepage: 65001
//     }],
//     outputCodepage : 65001,
//     searchingDelimiters: [';']
// }
// console.log(Converter.convert(options));  
await filesToXml.exit();
