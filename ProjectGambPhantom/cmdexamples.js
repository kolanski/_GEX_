var exec = require('child_process').exec;
var cmd = "tasklist /FI \"STATUS eq running\"";

exec(cmd, function (error, stdout, stderr) {
    //console.log(stdout);
    resultdata(stdout, ["explorer.exe", "slack.exe"]);
    //console.log(error);
    console.log(stderr);
});

function resultdata(str,args)
{
    for (var str1 of args) {
        console.log(str1 + " count is:" + (str.split(str1).length - 1));
    }
}