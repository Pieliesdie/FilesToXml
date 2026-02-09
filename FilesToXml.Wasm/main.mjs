const {default: Engine, Converter} = await import('./bin/filesToXml/filesToXml.js');
const fs = await import('node:fs');

class FilesToXmlApi {
    constructor(converter) {
        this.converter = converter;
    }

    convert = options => this.converter.convert(options);
    beautify = xml => this.converter.beautify(xml);
}
class FilesToXmlFactory {
    static BootStatus = {
        Standby: 0,
        Booting: 1,
        Booted: 2
    };

    static async #waitForBoot(engine) {
        const sleep = ms => new Promise(r => setTimeout(r, ms));
        while (engine.getStatus() === this.BootStatus.Booting) {
            await sleep(100);
        }
    }

    static async create() {
        const bootStatus = Engine.getStatus();

        if (bootStatus === this.BootStatus.Standby) {
            await Engine.boot();
        } else if (bootStatus === this.BootStatus.Booting) {
            await this.#waitForBoot(Engine);
        }

        return new FilesToXmlApi(Converter);
    }
}

const csvPath = '../FilesToXml.Tests/Files/csv.csv';
const xlsxPath = '../FilesToXml.Tests/Files/xlsx.xlsx';
const options =
    {
        disableFormat: true,
        Files: [
            {
                path: "csv.csv",
                data: fs.readFileSync(csvPath).toString('base64'),
                codePage: 1251,
                label: 'csv-file',
                delimiter: 'auto'
            },
            {
                path: "xlsx.xlsx",
                data: fs.readFileSync(xlsxPath).toString('base64')
            }
        ]
    }

console.log('Creating converter...');
let converter = await FilesToXmlFactory.create();
console.log('Calling convert function...');
let convertedContent = (await converter.convert(options)).result;
let beautifyContent = await converter.beautify(convertedContent);
console.log(beautifyContent);