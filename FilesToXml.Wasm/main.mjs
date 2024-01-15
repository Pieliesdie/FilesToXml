const api = await loadApi();
console.log(api.getBackendName())
console.log(api.beautify("<Root><test t=\"123123\"/></Root>"))
async function loadApi(){
    const { default: filesToXml, Global } = await import("./bin/filesToXml/filesToXml.mjs");
    const bootStatus = filesToXml.getStatus();
    if(bootStatus === 0 /*Standby*/) {
        await filesToXml.boot();
    }
    else if(bootStatus === 1 /*Booting*/ ){
        const sleep = ms => new Promise(r => setTimeout(r, ms));
        while(filesToXml.getStatus() === 1){
            await sleep(100);
        }
    }
    return Global;
}