const path = require('path');
const webpack = require("webpack");
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
    mode: 'development',   
    entry: {
        //Analista
        CriarChamada: path.resolve(__dirname, 'Scripts/Areas/Analista/CriarChamada.js'),
        ChamadasAbertas: path.resolve(__dirname, 'Scripts/Areas/Analista/ChamadasAbertas.js'),
        EditarChamada: path.resolve(__dirname, 'Scripts/Areas/Analista/EditarChamada.js'),
        AnalistaDashboard: path.resolve(__dirname, 'Scripts/Areas/Analista/DashBoard.js'),
        RelatorioAnalista: path.resolve(__dirname, 'Scripts/Areas/Analista/Relatorios.js'),
        AlterarConfig: path.resolve(__dirname, 'Scripts/Areas/Analista/AlterarConfig.js'),
        //Usuario
        Layout: path.resolve(__dirname, 'Scripts/Areas/Usuario/Layout.js'),
        Binance: path.resolve(__dirname, 'Scripts/Areas/Usuario/Binance/SetKeys.js'),
        Dashboard: path.resolve(__dirname, 'Scripts/Areas/Usuario/DashBoard/DashBoard.js'),
        CompraBNB: path.resolve(__dirname, 'Scripts/Areas/Usuario/ComprarBNB/ComprarBNB.js'),
        Pagamentos: path.resolve(__dirname, 'Scripts/Areas/Usuario/Pagamentos/index.js'),
        ConfirmEmail: path.resolve(__dirname, 'Scripts/Areas/Usuario/Register/ConfirmEmail.js'),
        Geral: path.resolve(__dirname, 'Scripts/Areas/Usuario/Relatorios/Geral.js'),
        Individual: path.resolve(__dirname, 'Scripts/Areas/Usuario/Relatorios/Individual.js'),
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, './Scripts/Bundle'),
        sourceMapFilename: "[file].map?[contenthash]"
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, 'css-loader']
            }
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            '$': 'jquery',
            jQuery: "jquery",           
            "window.jQuery": "jquery"         
        }),
        new MiniCssExtractPlugin({
            // Options similar to the same options in webpackOptions.output
            // both options are optional
            filename: '[name].css',
            chunkFilename: '[id].css',
        }),
        new CleanWebpackPlugin()
    ],
    devtool: 'source-map'
};