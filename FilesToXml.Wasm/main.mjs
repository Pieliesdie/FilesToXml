import fs from 'node:fs'

let csvPath = "E:/git/FilesToXml/FilesToXml.Tests/Files/csv.csv";
let xlsxPath = "E:/git/FilesToXml/FilesToXml.Tests/Files/xlsx.xlsx";
const options =
    {
        disableFormat: false,
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

console.log(await convert(options));

async function convert(options) {
    let api = await loadApi();
    return api.convert(options);

    async function loadApi() {
        const BootStatus = {
            Standby: 0,
            Booting: 1,
            Booted: 2
        };
        const {default: filesToXml, Converter} = await import("./bin/filesToXml/index.mjs");
        const bootStatus = filesToXml.getStatus();
        if (bootStatus === BootStatus.Standby) {
            await filesToXml.boot();
        } else if (bootStatus === BootStatus.Booting) {
            const sleep = ms => new Promise(r => setTimeout(r, ms));
            while (filesToXml.getStatus() === BootStatus.Booting) {
                await sleep(100);
            }
        } else if (bootStatus === BootStatus.Booted) {
            // Already booted, no need to do anything
        }
        return Converter;
    }
}