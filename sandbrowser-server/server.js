// Require express and create an instance of it
var express = require('express');
var app = express();

var bodyParser = require('body-parser');

app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded
app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE,OPTIONS');
  res.header("Access-Control-Allow-Headers", "X-Requested-With,Content-Type,Cache-Control");
  if (req.method === 'OPTIONS') {
   res.statusCode = 204;
   return res.end();
 } else {
   return next();
 }
});

// on the request to root (localhost:3000/)
app.get('/:path', function (req, res) {
  var fs = require('fs');
  var filename = req.params.path;

  console.log('path files/' + filename + '.json');
  
  fs.readFile('files/' + filename + '.json', 'utf8', function (err, data) {
    if (err) {
      res.status(404).send("Not Found");
      return;
    }

    var json = JSON.parse(data);
    res.json(json);
  });
});

// on the request to root (localhost:3000/)
app.post('/', function (req, res) {
  var file=req.body.file;
  var filename=req.body.filename;
  
  var fs = require('fs');
  fs.writeFile('files/' + filename + '.json', file, 'utf8', function (err) {
    if (err) {
      return console.log(err);
    }
    console.log("The file was saved!");
  });
  
  res.end("yes");
});

// start the server in the port 3000 !
app.listen(3000, function () {
  console.log('Example app listening on port 3000.');
});