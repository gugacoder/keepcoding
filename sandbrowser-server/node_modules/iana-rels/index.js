
var rels = require('./meta.json');
var camel = require('to-camel-case');

rels.forEach(function (rel) {
  exports[camel(rel.name)] = exports[rel.name] = rel;
});
