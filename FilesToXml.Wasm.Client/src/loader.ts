// @ts-ignore
import filesToXml, {Converter} from "../../FilesToXml.Wasm/bin/filesToXml";

async function waitForBoot() {
    const sleep = (ms: number) => new Promise(r => setTimeout(r, ms));
    while (filesToXml.getStatus() !== filesToXml.BootStatus.Booted) {
        await sleep(100);
    }
}

async function loadApi() {
    const currentStatus = filesToXml.getStatus();
    if (currentStatus === filesToXml.BootStatus.Standby) {
        await filesToXml.boot();
    } else if (currentStatus === filesToXml.BootStatus.Booting) {
        await waitForBoot();
    } else if (currentStatus === filesToXml.BootStatus.Booted) {
        // Already booted, no need to do anything
    }
}

export default Converter;