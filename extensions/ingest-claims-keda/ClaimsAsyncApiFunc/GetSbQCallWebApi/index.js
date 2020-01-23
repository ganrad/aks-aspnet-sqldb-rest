const request = require('request');

module.exports = async function(context, claimItem) {

    context.log('Nodejs function: GetSbQCallWebApi, received message. Start time: ', context.bindingData.enqueuedTimeUtc);
    context.log('GetSbQCallWebApi - DeliveryCount: ', context.bindingData.deliveryCount);
    context.log('GetSbQCallWebApi - MessageId: ', context.bindingData.messageId);
    context.log('GetSbQCallWebApi - Processing claim item: ', claimItem);

    var apiHost = process.env["ClaimsApiHost"];
    var apiPath = process.env["ClaimsApiEndpoint"];
    var apiMethod = process.env["ClaimsApiMethod"];
    context.log('GetSbQCallWebApi - Claims API Host: ', apiHost);
    context.log('GetSbQCallWebApi - Claims API Path: ', apiPath);
    context.log('GetSbQCallWebApi - Claims API Method: ', apiMethod);

    const data = JSON.stringify(claimItem);

    const options = {
      baseUrl: "http://" + apiHost,
      uri: apiPath,
      method: apiMethod,
      body: data,
      headers: {
        'Content-Type': 'application/json',
        'Content-Length': data.length
      }
    };
    let promise = new Promise((resolve,reject) => {
        request(options, function(err, res, body) {
          context.log('GetSbQCallWebApi - Http status code:', res && res.statusCode);

	  if ( !err && (res.statusCode >= 200 && res.statusCode < 400) ) { // Http success operation returns +200!
    	    context.log('GetSbQCallWebApi - Http (Json) response:' + body);
            context.bindings.sbQueue = body;
            resolve(body);
	  }
          else {
	    context.log('GetSbQCallWebApi - Http exception:' + err);
	    resolve("");
	  };
        });
    });
    let claimJson = await promise;
    context.done();
};
