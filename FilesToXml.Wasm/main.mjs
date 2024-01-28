import filesToXml, {Converter} from "./bin/filesToXml/index.mjs";
import fs from 'node:fs'

let path = "F:/git/FilesToXml/FilesToXml.Tests/Files/json.json";
let data = new Uint8Array(await fs.readFileSync(path).buffer);
await filesToXml.boot();
const options = [
    {
        i: [[1,2,3]],
        k: 2
    }
]
console.log(Converter.convert(data));
await filesToXml.exit();
