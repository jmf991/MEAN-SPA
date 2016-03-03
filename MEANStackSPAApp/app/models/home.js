// app/models/home.js
// grab the mongoose module
var mongoose = require('mongoose');

// define our home model
// module.exports allows us to pass this to other files when it is called
module.exports = mongoose.model('Home', {
    name: { type: String, default: '' }
});
//If you want more fields, feel free to add them here. Read up on the Mongoose docs to see all the things you can define