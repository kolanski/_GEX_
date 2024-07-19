var pinnacleAPI = require('pinnacle-sports-api');
var pinnacle = new pinnacleAPI('AG899837', 'jekich17!');
var options = { sportId: 33 };

pinnacle.getFixtures(options, function (err, response, body) {
    if (err) throw new Error(err);
    //console.log(body);
});