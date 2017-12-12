# iana-rels

> A list of [official IANA link relations][ref].

## Usage

The export is an object where each key is Link Relation Name converted to camelCase.
(eg: `create-form` is now `createForm`)

```js
var rels = require('iana-rels');

console.log(rels.self);
// {
//   name: 'self',
//   description: 'Conveys an identifier for the link\'s context.',
//   reference: '[RFC4287]',
//   notes: ''
// }
```

## Status

The current [list][ref] being exported was last updated `2015-01-21`. If you notice
the list gets updated again, we can simply update this module to reflect that.
(just submit an issue)


[ref]: http://www.iana.org/assignments/link-relations/link-relations.xhtml
