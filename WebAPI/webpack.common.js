//const path = require('path');
//const { CleanWebpackPlugin } = require('clean-webpack-plugin');
//const HtmlWebpackPlugin = require('html-webpack-plugin');

//module.exports = {
//    plugins: [
//        // new CleanWebpackPlugin(['dist/*']) for < v2 versions of CleanWebpackPlugin
//        new CleanWebpackPlugin(),
//        new HtmlWebpackPlugin({
//            title: 'Production'
//        })
//    ],
//    entry: {
//        dashboard: './Scripts/DashBoard/es6/main.js',
//        projectManagement: './Scripts/ProjectManagement/es6/main.js'
//    },
//    output: {
//        filename: '[name].js',
//        path: path.resolve(__dirname, './Scripts/build')
//    },
//    // IMPORTANT NOTE: If you are using Webpack 2 or above, replace "loaders" with "rules"
//    module: {
//        rules: [{
//            loader: 'babel-loader',
//            test: /\.js$/,
//            exclude: /node_modules/
//        }]
//    }
//};