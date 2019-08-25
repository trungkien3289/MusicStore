const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    entry: {
        pageHeader: './Scripts/PageHeader/es6/main.js',
        login: './Scripts/Login/es6/main.js',
        register: './Scripts/Register/es6/main.js',
        dashboard: './Scripts/DashBoard/es6/main.js',
        projectManagement: './Scripts/ProjectManagement/es6/main.js',
        taskRequest: './Scripts/TaskRequest/es6/main.js'
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
        },
        {
            // Apply rule for .sass, .scss or .css files
            test: /\.(sa|sc|c)ss$/,

            // Set loaders to transform files.
            // Loaders are applying from right to left(!)
            // The first loader will be applied after others
            use: [
                {
                    loader: "style-loader" // creates style nodes from JS strings
                },
                {
                    loader: "css-loader" // translates CSS into CommonJS
                },
                {
                    loader: "sass-loader" // compiles Sass to CSS
                }
            ]
        }
        ]
    },
    plugins: [
    ]
}