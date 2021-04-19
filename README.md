# Better-SQL-Format - A JavaScript library to format T-SQL

This is a JavaScript library to format T-SQL. The source code is written in F# which is then compiled to JavaScript with [Fable](https://fable.io).

## Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 3.0 or higher
* [node.js](https://nodejs.org) with [npm](https://www.npmjs.com/)
* An F# editor like Visual Studio, Visual Studio Code with [Ionide](http://ionide.io/) or [JetBrains Rider](https://www.jetbrains.com/rider/).

## Installing dependencies

Run `npm install`

## Project structure

### SQL formatting code

The logic that formats SQL code is in the `src/lib` folder.

### Tests

Unit tests are in the `src/lib.tests` folder.  
Run tests with `npm run test`

### Generated JavaScript library

The generated `better-sql-format.js` JavaScript library is in the `public` folder.  
Update the library with `npm run build`
