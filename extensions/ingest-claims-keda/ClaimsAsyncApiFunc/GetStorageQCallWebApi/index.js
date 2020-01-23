const http = require('http');

module.exports = async function (context, claimItem) {
    var timeStamp = new Date().toISOString();

    context.log('ReadQCallWebApi function called. Start time: ', timeStamp);
    context.log('Processing work item: ', claimItem);

    var apiHost = process.env["ClaimsApiHost"];
    context.log('Claims API Host: ', apiHost);

    const data = JSON.stringify(claimItem);

    const options = {
      hostname: apiHost,
      port: 80,
      path: '/api/v1/claims',
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Content-Length': data.length
      }
    }

    const req = http.request(options, res => {
      console.log(`Http statusCode: ${res.statusCode}`)

      res.on('data', d => {
        process.stdout.write('Http response: ' + d)
      });

      res.on('end', () => {
	console.log("\nEnd response");
      });
    });

    req.on('error', error => {
      console.error('Encountered exception: ' + error)
    })

    req.write(data);
    req.end();
};
