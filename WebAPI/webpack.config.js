const path = require('path');

module.exports = {
    entry: {
        dashboard: './Scripts/DashBoard/es6/main.js',
        projectManagement: './Scripts/ProjectManagement/es6/main.js'
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, './Scripts/build')
    },
    devtool: 'inline-source-map',
    // IMPORTANT NOTE: If you are using Webpack 2 or above, replace "loaders" with "rules"
    module: {
        rules: [{
            loader: 'babel-loader',
            test: /\.js$/,
            exclude: /node_modules/
        }]
    }
}