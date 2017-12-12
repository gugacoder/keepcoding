
var csv = require('csv-parse');
var fs = require('fs');
var path = require('path');

var src = path.resolve(__dirname, '../meta.csv'); // TODO: download CSV
var dest = path.resolve(__dirname, '../meta.json')

var columns = [ 'name', 'description', 'reference', 'notes' ];
var input = fs.readFileSync(src, 'utf8');

csv(input, { columns: columns }, function (err, rels) {
  rels.shift(); // drop the headers
  rels.forEach(function (rel) {
    // reduce whitespace
    rel.description = rel.description.replace(/\s+/g, ' ');
    rel.notes = rel.notes.replace(/\s+/g, ' ');
  });

  fs.writeFileSync(dest, JSON.stringify(rels, null, 2));
});
