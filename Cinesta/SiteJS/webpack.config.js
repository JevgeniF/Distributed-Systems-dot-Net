const path = require('path');
const webpack = require('webpack');
// const HtmlPlugin = require('html-webpack-plugin');
const CopyPlugin = require("copy-webpack-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const TerserPlugin = require("terser-webpack-plugin");

module.exports = {
    mode: 'production',
    entry: {
        site: "./src/site.ts",
        'jquery.validate.globalize': './src/jquery.validate.globalize.js'
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: "[name].js",
        publicPath: '',
    },
    optimization: {
        minimize: true,
        minimizer: [
            new TerserPlugin({
                extractComments: false,
            }),
        ],
    },
    module: {
        rules: [{
            test: /\.tsx?$/,
            use: 'ts-loader',
            exclude: /node_modules/,
        },
            {
                test: /\.css$/i,
                use: [MiniCssExtractPlugin.loader, 'css-loader'],
            },
            {
                test: /\.exec\.js$/,
                use: 'script-loader'
            },
            {
                test: /\.(png|jpg|gif)$/i,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 10240,
                    },
                }, ],
            },
            {
                test: /\.woff(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 10240,
                        mimetype: 'application/font-woff'
                    },
                }, ],
            },
            {
                test: /\.woff2(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 10240,
                        mimetype: 'application/font-woff2'
                    },
                }, ],
            },

            {
                test: /\.ttf(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 10240,
                        mimetype: 'application/octet-stream'
                    },
                }, ],
            },
            {
                test: /\.svg(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 10240,
                        mimetype: 'image/svg+xml'
                    },
                }, ],
            }, {
                test: /\.(eot|otf)(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
                use: [{
                    loader: 'file-loader',
                    options: {
                        esModule: false
                    },
                }, ],
            },

        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
        alias: {
            'cldr$': 'cldrjs',
            'cldr': 'cldrjs/dist/cldr'
        },
    },
    plugins: [
        /*
        new HtmlPlugin({
            template: "./src/index.html",
            inject: "body",
            minify: false,
        }),
        */
        new MiniCssExtractPlugin(),
        new CleanWebpackPlugin(),
        new CopyPlugin({
            patterns: [
                { from: 'node_modules/cldr-core/supplemental/likelySubtags.json', to: 'cldr-core/supplemental' },
                { from: 'node_modules/cldr-core/supplemental/numberingSystems.json', to: 'cldr-core/supplemental' },
                { from: 'node_modules/cldr-core/supplemental/timeData.json', to: 'cldr-core/supplemental' },
                { from: 'node_modules/cldr-core/supplemental/weekData.json', to: 'cldr-core/supplemental' },

                { from: 'node_modules/cldr-numbers-modern/main/et/', to: 'cldr-numbers-modern/main/et/' },
                { from: 'node_modules/cldr-dates-modern/main/et/', to: 'cldr-dates-modern/main/et/' },

                { from: 'node_modules/cldr-numbers-modern/main/en-GB/', to: 'cldr-numbers-modern/main/en/' },
                { from: 'node_modules/cldr-dates-modern/main/en-GB/', to: 'cldr-dates-modern/main/en/' },

                { from: 'node_modules/cldr-numbers-modern/main/ru/', to: 'cldr-numbers-modern/main/ru/' },
                { from: 'node_modules/cldr-dates-modern/main/ru/', to: 'cldr-dates-modern/main/ru/' },
            ]
        }),

    ]
}
