/// <binding ProjectOpened='watch' />

var gulp = require('gulp')
var concat = require('gulp-concat')
var watch = require('gulp-watch')
var jasmine = require('gulp-jasmine');

//var project = require('./project.json');

gulp.task('build', function () {
    gulp.src(['App/app.js', 'App/Components/*.js', 'App/Directives/*.js', 'App/Services/*.js', 'App/Controllers/*.js', 'App/ProfileChart/*.js'])
      .pipe(concat('altidude.js'))
      .pipe(gulp.dest('Scripts'))
})

//gulp.task('specs', function () {
//    gulp.src(['Specs/*Spec.js'])
//      .pipe(concat('altidudeSpecs.js'))
//      .pipe(gulp.dest('Specs'))
//})

gulp.task('watch', ['build'], function () {
    gulp.watch('App/**/*.js', ['build'])
})


//gulp.task('runtest', function () {
//     return gulp.src('Specs/altidudeSpecs.js')
//        .pipe(jasmine());
//});
 
//gulp.task('jasmine', function () {
//    var filesForTest = ['Scripts/jquery-1.10.2.js',
//        'Scripts/angular.min.js',
//        'Scripts/ng-file-upload.js',
//        'Scripts/altidude.js',
//        'Specs/altidudeSpecs.js'];

//    return gulp.src(filesForTest)
//    .pipe(watch(filesForTest))
//    .pipe(jasmineBrowser.specRunner())
//    .pipe(jasmineBrowser.server({port: 8888}));
//});