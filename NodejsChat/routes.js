var passport = require('passport');
var Account = require('./models/account');

module.exports = function (app) {
    
    app.get('/', function (req, res) {
        res.render('index', { user : req.user });
    });
    
    app.get('/register', function (req, res) {
        res.render('register', {});
    });
    
    app.post('/register', function (req, res) {
        Account.register(new Account({ username : req.body.username }), req.body.password, function (err, account) {
            if (err) {
                return res.render('register', { account : account });
            }
            
            passport.authenticate('local')(req, res, function () {
                res.redirect('/');
            });
        });
    });
    
    app.get('/login', function (req, res) {
        res.render('login', { user : req.user });
    });
    
    app.post('/login', passport.authenticate('local'), function (req, res) {
        res.redirect('/');
    });
    
    app.get('/logout', function (req, res) {
        req.logout();
        res.redirect('/');
    });
    function ensureAuthenticated(req, res, next) {
        
        var isAuth = req.isAuthenticated();
        
        if (req.isAuthenticated()) {
            return next();
        }
        res.redirect('/login');
    }
    app.get('/ping', function (req, res) {
        res.send("pong!", 200);
    });
    app.get('/fork', ensureAuthenticated, function (req, res) {
        res.render("page2" ,{ user : req.user });
    });
    app.get('/restarter', ensureAuthenticated, function (req, res) {
        res.render("pagerestart", { user: req.user });
    });
    app.get('/table', ensureAuthenticated, function (req, res) {
        res.render("page3");
    });
};