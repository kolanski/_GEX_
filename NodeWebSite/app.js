var express = require('express');
var app = express();
app.listen(8080);

var names = {
    'john': 'admin', 
    'mike': 'manager'
};

app.get('/user/:name', function (request, response) {
    var role = names[request.params.name];
    if (request.params.name in names)
        role = names[request.params.name]
    else
        role = 'unknown role';
    response.write(role);
    response.end();
}
);

app.get("/", function (request, response) {

})
