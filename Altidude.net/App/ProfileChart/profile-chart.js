/// <reference path="../definitions/snap.svg.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var ProfileChart;
(function (ProfileChart) {
    "use strict";
    var Point = (function () {
        function Point(x, y) {
            this.x = x;
            this.y = y;
        }
        Point.prototype.offset = function (vector) {
            return new Point(this.x + vector.x, this.y + vector.y);
        };
        Point.prototype.perpendicularDistanceTo = function (start, end) {
            var a = this.x - start.x;
            var b = this.y - start.y;
            var c = end.x - start.x;
            var d = end.y - start.y;
            return Math.abs(a * d - c * b) / Math.sqrt(c * c + d * d);
        };
        Point.prototype.heightDistanceTo = function (start, end) {
            var k = (end.y - start.y) / (end.x - start.x);
            var y = start.y + k * (this.x - start.x);
            return Math.abs(this.y - y);
        };
        return Point;
    }());
    ProfileChart.Point = Point;
    var Vector = (function () {
        function Vector(x, y) {
            this.x = x;
            this.y = y;
        }
        Vector.combineNormalized = function (first, second) {
            var vector = new Vector(first.x + second.x, first.y + second.y);
            var dotProduct = first.x * second.x + first.y * second.y;
            var scale = 1 / (dotProduct + 1);
            return new Vector(vector.x * scale, vector.y * scale);
        };
        Vector.prototype.normalize = function () {
            var length = Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));
            return new Vector(this.x / length, this.y / length);
        };
        Vector.prototype.scaleTo = function (scale) {
            return new Vector(this.x * scale, this.y * scale);
        };
        Vector.prototype.scaleToX = function (x) {
            var k = this.y / this.x;
            return new Vector(x, x * k);
        };
        Vector.prototype.scale = function (scaleX, scaleY) {
            return new Vector(this.x * scaleX, this.y * scaleY);
        };
        Vector.prototype.add = function (vector) {
            return new Vector(this.x + vector.x, this.y + vector.y);
        };
        Vector.prototype.reverse = function () {
            return new Vector(-this.x, -this.y);
        };
        Vector.prototype.getLength = function () {
            return Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));
        };
        // static CalcCrossProduct(first: Vector, second: Vector): Vector {
        //     return new Vector(first.x * second.y) - (first.y * second.y);
        // }
        Vector.create = function (from, to) {
            return new Vector(to.x - from.x, to.y - from.y);
        };
        Vector.createNormalized = function (from, to) {
            return new Vector(to.x - from.x, to.y - from.y).normalize();
        };
        Vector.createNormal = function (from, to) {
            var dx = to.x - from.x;
            var dy = to.y - from.y;
            return new Vector(dy, -dx);
        };
        return Vector;
    }());
    ProfileChart.Vector = Vector;
    var Offset = (function () {
        function Offset(width, height) {
            this.width = width;
            this.height = height;
        }
        return Offset;
    }());
    var Size = (function () {
        function Size() {
        }
        return Size;
    }());
    var Margin = (function () {
        function Margin(left, top, right, bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        return Margin;
    }());
    ProfileChart.Margin = Margin;
    var Rectangle = (function () {
        function Rectangle(x, y, width, height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        Rectangle.prototype.getCenter = function () {
            return new Point(this.x + this.width / 2, this.y + this.height / 2);
        };
        Rectangle.prototype.apply = function (margin) {
            return new Rectangle(this.x + margin.left, this.y + margin.top, this.width - margin.left - margin.right, this.height - margin.top - margin.bottom);
        };
        return Rectangle;
    }());
    ProfileChart.Rectangle = Rectangle;
    var Extent = (function () {
        function Extent(points) {
            this.minX = Number.MAX_VALUE;
            this.maxX = -Number.MAX_VALUE;
            this.minY = Number.MAX_VALUE;
            this.maxY = -Number.MAX_VALUE;
            this.containPoints(points);
        }
        Extent.prototype.containPoint = function (point) {
            this.minX = Math.min(this.minX, point.x);
            this.maxX = Math.max(this.maxX, point.x);
            this.minY = Math.min(this.minY, point.y);
            this.maxY = Math.max(this.maxY, point.y);
        };
        Extent.prototype.containPoints = function (points) {
            for (var i = 0; i < points.length; i++) {
                this.containPoint(points[i]);
            }
        };
        Extent.prototype.getWidth = function () {
            return this.maxX - this.minX;
        };
        Extent.prototype.getHeight = function () {
            return this.maxY - this.minY;
        };
        Extent.prototype.getRatio = function () {
            return this.getWidth() / this.getHeight();
        };
        Extent.prototype.toRectangle = function () {
            return new Rectangle(this.minX, this.minY, this.maxX - this.minX, this.maxY - this.minY);
        };
        return Extent;
    }());
    var ElapsedTime = (function () {
        function ElapsedTime(seconds) {
            this.seconds = seconds;
        }
        ElapsedTime.prototype.pad = function (num, size) {
            var s = num + "";
            while (s.length < size) {
                s = "0" + s;
            }
            return s;
        };
        ElapsedTime.prototype.padZeros = function (num) {
            return this.pad(num, 2);
        };
        ElapsedTime.prototype.getSeparator = function (elapsedTime) {
            return elapsedTime.length > 0 ? ":" : "";
        };
        ElapsedTime.prototype.formatTime = function (elapsedTime, time, includeZero) {
            var timeString = "";
            if (includeZero || time > 0) {
                timeString += this.getSeparator(elapsedTime);
                timeString += this.padZeros(time);
            }
            return timeString;
        };
        ElapsedTime.prototype.toString = function () {
            var seconds = this.seconds;
            var days = Math.floor(seconds / (60 * 60 * 24));
            seconds -= days * (60 * 60 * 24);
            var hours = Math.floor(seconds / (60 * 60));
            seconds -= hours * (60 * 60);
            var minutes = Math.floor(seconds / (60));
            seconds -= minutes * (60);
            hours = days * 24 + hours;
            var elapsedTime = "";
            // elapsedTime += this.formatTime(elapsedTime, days, false);
            elapsedTime += this.formatTime(elapsedTime, hours, false);
            elapsedTime += this.formatTime(elapsedTime, minutes, true);
            elapsedTime += this.formatTime(elapsedTime, seconds, true);
            return elapsedTime;
        };
        return ElapsedTime;
    }());
    var ChartType = (function () {
        // transformData(data: ChartData, chartArea: Rectangle)
        // {
        //    var transform = new TransformProcessor(chartArea);
        //    data.courseProfile = transform.Process(data.courseProfile);
        //    return data;
        // }
        function ChartType(name, templateUrl) {
            this.name = name;
            this.templateUrl = templateUrl;
        }
        return ChartType;
    }());
    var ControlPointValues = (function () {
        function ControlPointValues(p1, p2) {
            this.p1 = p1;
            this.p2 = p2;
        }
        return ControlPointValues;
    }());
    var Chart = (function () {
        function Chart(id, name, templateUrl) {
            this.id = id;
            this.name = name;
            this.templateUrl = templateUrl;
            this.surfaceArea = new Rectangle(0, 0, 1900, 1000);
        }
        Chart.prototype.clearChart = function () {
            Snap.selectAll("SVG > *:not(defs)").remove();
        };
        Chart.prototype.getTemplateUrl = function () {
            return this.templateUrl;
        };
        Chart.prototype.createCurveToPath = function (points) {
            var path = "M" + points[0].x + " " + points[0].y;
            var operation = "C";
            for (var i = 1; i < points.length - 1; i++) {
                path += operation + points[i - 1].x + "," + points[i - 1].y + " " +
                    points[i + 1].x + "," + points[i + 1].y + " " +
                    points[i].x + "," + points[i].y;
            }
            return path;
        };
        Chart.prototype.toPathString = function (points) {
            var path = "";
            var operation = "M";
            for (var i = 0; i < points.length; i++) {
                path += operation + points[i].x + " " + points[i].y;
                if (i === 0) {
                    operation = "L";
                }
            }
            return path;
        };
        Chart.prototype.createCurveToPathT = function (points) {
            var path = "";
            var operation = "M";
            for (var i = 0; i < points.length; i++) {
                path += operation + points[i].x + " " + points[i].y;
                if (i === 0) {
                    operation = "T";
                }
            }
            return path;
        };
        Chart.prototype.createCurveThroughPath = function (points) {
            var path = "";
            var x = new Array();
            var y = new Array();
            for (var i = 0; i < points.length; i++) {
                x[i] = points[i].x;
                y[i] = points[i].y;
            }
            /*computes control points p1 and p2 for x and y direction*/
            var px = this.computeControlPoints(x);
            var py = this.computeControlPoints(y);
            for (var j = 1; j < points.length; j++) {
                var controlPoint1 = new Point(px.p1[j - 1], py.p1[j - 1]);
                var controlPoint2 = new Point(px.p2[j - 1], py.p2[j - 1]);
                path += this.curvePath(points[j - 1], controlPoint1, controlPoint2, points[j]);
            }
            return path;
        };
        /*creates formated path string for SVG cubic path element*/
        Chart.prototype.curvePath = function (from, controlPoint1, controlPoint2, to) {
            return "M " + from.x + "," + from.y + " C " + controlPoint1.x + "," + controlPoint1.y + " " + controlPoint2.x + "," + controlPoint2.y + " " + to.x + "," + to.y;
        };
        /*computes control points given knots K, this is the brain of the operation*/
        Chart.prototype.computeControlPoints = function (k) {
            var p1 = new Array();
            var p2 = new Array();
            var n = k.length - 1;
            /*rhs vector*/
            var a = new Array();
            var b = new Array();
            var c = new Array();
            var r = new Array();
            /*left most segment*/
            a[0] = 0;
            b[0] = 2;
            c[0] = 1;
            r[0] = k[0] + 2 * k[1];
            /*internal segments*/
            for (var i = 1; i < n - 1; i++) {
                a[i] = 1;
                b[i] = 4;
                c[i] = 1;
                r[i] = 4 * k[i] + 2 * k[i + 1];
            }
            /*right segment*/
            a[n - 1] = 2;
            b[n - 1] = 7;
            c[n - 1] = 0;
            r[n - 1] = 8 * k[n - 1] + k[n];
            /*solves Ax=b with the Thomas algorithm (from Wikipedia)*/
            for (var i = 1; i < n; i++) {
                var m = a[i] / b[i - 1];
                b[i] = b[i] - m * c[i - 1];
                r[i] = r[i] - m * r[i - 1];
            }
            p1[n - 1] = r[n - 1] / b[n - 1];
            for (var i = n - 2; i >= 0; --i) {
                p1[i] = (r[i] - c[i] * p1[i + 1]) / b[i];
            }
            /*we have p1, now compute p2*/
            for (var i = 0; i < n - 1; i++) {
                p2[i] = 2 * k[i + 1] - p1[i + 1];
            }
            p2[n - 1] = 0.5 * (k[n] + p1[n - 1]);
            // return { p1: p1, p2: p2 };
            return new ControlPointValues(p1, p2);
        };
        Chart.prototype.calcAlignmentVector = function (el, horizontal, vertical) {
            var bbox = el.getBBox();
            var x = 0.0;
            var y = 0.0;
            if (horizontal === HorizontalAlignment.Center) {
                x = bbox.width / -2;
            }
            else if (horizontal === HorizontalAlignment.Right) {
                x = -bbox.width;
            }
            if (vertical === VerticalAlignment.Middle) {
                y = bbox.height / 2;
            }
            else if (vertical === VerticalAlignment.Top) {
                y = bbox.height;
            }
            return new Vector(x, y);
        };
        Chart.prototype.align = function (el, alignment, position) {
            var pos = position.offset(this.calcAlignmentVector(el, alignment.horizontal, alignment.vertical));
            el.attr({ x: pos.x, y: pos.y });
        };
        Chart.prototype.fitTextInto = function (text, startSize, maxWidth) {
            var bbox = text.getBBox();
            for (var fontSize = startSize; bbox.width > maxWidth; fontSize--) {
                text.attr({ fontSize: fontSize + "px" });
                bbox = text.getBBox();
            }
        };
        return Chart;
    }());
    ProfileChart.Chart = Chart;
    var HorizontalAlignment;
    (function (HorizontalAlignment) {
        HorizontalAlignment[HorizontalAlignment["Left"] = 0] = "Left";
        HorizontalAlignment[HorizontalAlignment["Center"] = 1] = "Center";
        HorizontalAlignment[HorizontalAlignment["Right"] = 2] = "Right";
    })(HorizontalAlignment || (HorizontalAlignment = {}));
    var VerticalAlignment;
    (function (VerticalAlignment) {
        VerticalAlignment[VerticalAlignment["Top"] = 0] = "Top";
        VerticalAlignment[VerticalAlignment["Middle"] = 1] = "Middle";
        VerticalAlignment[VerticalAlignment["Bottom"] = 2] = "Bottom";
    })(VerticalAlignment || (VerticalAlignment = {}));
    var Alignment = (function () {
        function Alignment(horizontal, vertical) {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }
        return Alignment;
    }());
    Alignment.leftTop = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);
    Alignment.leftMiddle = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Middle);
    Alignment.leftBottom = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
    Alignment.centerTop = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Top);
    Alignment.centerMiddle = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Middle);
    Alignment.centerBottom = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
    Alignment.rightTop = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Top);
    Alignment.rightMiddle = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Middle);
    Alignment.rightBottom = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Bottom);
    ProfileChart.Alignment = Alignment;
    var Text = (function () {
        function Text() {
        }
        Text.calcAlignmentVector = function (el, horizontal, vertical) {
            var bbox = el.getBBox();
            var x = 0.0;
            var y = 0.0;
            if (horizontal === HorizontalAlignment.Center) {
                x = bbox.width / -2;
            }
            else if (horizontal === HorizontalAlignment.Right) {
                x = -bbox.width;
            }
            if (vertical === VerticalAlignment.Middle) {
                y = bbox.height / 2;
            }
            else if (vertical === VerticalAlignment.Top) {
                y = bbox.height;
            }
            return new Vector(x, y);
        };
        Text.align = function (el, alignment, position) {
            var pos = position.offset(this.calcAlignmentVector(el, alignment.horizontal, alignment.vertical));
            el.attr({ x: pos.x, y: pos.y });
        };
        Text.fitInside = function (text, startSize, maxWidth) {
            var bbox = text.getBBox();
            for (var fontSize = startSize; bbox.width > maxWidth; fontSize--) {
                text.attr({ fontSize: fontSize + "px" });
                bbox = text.getBBox();
            }
        };
        Text.renderInside = function (paper, text, position, alignment, fontSize, maxWidth, params) {
            var textElement = paper.text(position.x, position.y, text);
            textElement.attr(params);
            this.fitInside(textElement, fontSize, maxWidth);
            this.align(textElement, alignment, position);
            return textElement;
        };
        Text.render = function (paper, text, position, alignment, params) {
            var textElement = paper.text(position.x, position.y, text);
            textElement.attr(params);
            this.align(textElement, alignment, position);
            return textElement;
        };
        return Text;
    }());
    var LoadingChart = (function (_super) {
        __extends(LoadingChart, _super);
        function LoadingChart() {
            return _super.call(this, LoadingChart.id, "Loading", "/App/ProfileChart/Templates/loading.svg") || this;
        }
        LoadingChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("l(0, 1, 1, 0)#bebebe-#fefefe");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        };
        LoadingChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#loadingChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, surfaceArea);
            Text.render(paper, "Loading...", surfaceArea.getCenter(), Alignment.centerMiddle, { fontSize: "72px" });
        };
        return LoadingChart;
    }(Chart));
    LoadingChart.id = "73CE29D6-AE8F-405C-BA21-C267F81AEFC5";
    ProfileChart.LoadingChart = LoadingChart;
    var TestChart = (function (_super) {
        __extends(TestChart, _super);
        function TestChart() {
            return _super.call(this, TestChart.id, "Connecting dots", "/App/ProfileChart/Templates/sdftest.svg") || this;
        }
        TestChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("l(0, 1, 1, 0)#bebebe-#fefefe");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        };
        TestChart.prototype.renderHeader = function (paper, data, area) {
            var textPoint = new Point(area.x + area.width / 2, area.y);
            var courseNameText = paper.text(textPoint.x, textPoint.y, data.courseName);
            courseNameText.attr({ textAnchor: "middle", fontSize: "20px", fill: "#2ba7de" });
            var bbox = courseNameText.getBBox();
            courseNameText.attr({ y: textPoint.y + bbox.height });
        };
        TestChart.prototype.renderProfile = function (paper, data, profileArea) {
            var extent = new Extent([]);
            extent.containPoints(data.courseProfile);
            var pipeline = new PointProcessorPipeLine();
            var reduce = new ReduceToNumberProcessor(50);
            pipeline.add(reduce);
            var profile = pipeline.process(data.courseProfile);
            // for (var i = 0; i < data.legs.length; i++) {
            //    if (i == 0)
            //        profile.push(transform.processPoint(data.splits[i].point));
            //    var legprofile = reduce.process(transform.process(data.legs[i].profile));
            //    //var legprofile = pipeline.Process(data.legs[i].profile);
            //    courseProfile.push.apply(courseProfile, transform.process(data.legs[i].profile));
            //    for (var j = 1; j < legprofile.length - 1; j++)
            //        profile.push(legprofile[j]);
            //    profile.push(transform.processPoint(data.splits[i + 1].point));
            // }
            var color = "#666666";
            paper.path(_super.prototype.toPathString.call(this, profile)).attr({ fill: "none", stroke: color, strokeWidth: 1 });
            for (var i = 0; i < profile.length; i++) {
                var dot = paper.circle(profile[i].x, profile[i].y, 3);
                dot.attr({ fill: "#333333" });
                var prev = profile[i - 1];
                var point = profile[i];
                var next = profile[i + 1];
                var textOffset = this.calcTextOffset(prev, point, next, 15);
                var textPoint = profile[i].offset(textOffset);
                var text = paper.text(textPoint.x, textPoint.y, i.toString());
                text.attr({ textAnchor: "middle", dominantBaseline: "middle", fill: "#333333", fontSize: 13 });
            }
        };
        TestChart.prototype.calcTextOffset = function (prev, point, next, distance) {
            var direction;
            if (prev === undefined) {
                direction = Vector.createNormal(point, next);
            }
            else if (next === undefined) {
                direction = Vector.createNormal(prev, point);
            }
            else {
                var firstVector = Vector.createNormalized(prev, point);
                var secondVector = Vector.createNormalized(next, point);
                direction = firstVector.add(secondVector);
            }
            var vector = direction.normalize().scaleTo(distance);
            return vector;
        };
        TestChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var widthMargin = surfaceArea.width / 20;
            var heightMargin = surfaceArea.height / 3;
            var headerArea = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin * 2));
            var profileArea = surfaceArea.apply(new Margin(widthMargin, heightMargin, widthMargin, heightMargin));
            var data = new ChartData(profile, result, profileArea);
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#testChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, surfaceArea);
            this.renderHeader(paper, data, headerArea);
            this.renderProfile(paper, data, profileArea);
        };
        return TestChart;
    }(Chart));
    TestChart.id = "4F0B4EC0-6E72-44FD-9F26-F4423D7CE973";
    ProfileChart.TestChart = TestChart;
    var MountainSilhouetteChart = (function (_super) {
        __extends(MountainSilhouetteChart, _super);
        function MountainSilhouetteChart() {
            return _super.call(this, MountainSilhouetteChart.id, "Mountain Silhouette", "/App/ProfileChart/Templates/mountain_silhouette.svg") || this;
        }
        MountainSilhouetteChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("l(0, 1, 1, 0)#DDD6C6-#fefefe");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        };
        MountainSilhouetteChart.prototype.renderMountain = function (paper, topArea, baseY, color) {
            var points = new MountainGenerator(topArea, 2, 5).points;
            var profileBody = [];
            profileBody.push(new Point(topArea.x, topArea.y));
            profileBody.push.apply(profileBody, points);
            profileBody.push(new Point(topArea.x + topArea.width, points[points.length - 1].y));
            profileBody.push(new Point(topArea.x + topArea.width, baseY));
            profileBody.push(new Point(topArea.x, baseY));
            profileBody.push(new Point(topArea.x, points[0].y));
            var bodyPathString = _super.prototype.toPathString.call(this, profileBody) + " Z";
            paper.path(bodyPathString).attr({ fill: color });
        };
        MountainSilhouetteChart.prototype.renderProfile = function (paper, data, chartArea) {
            var reduce = new ReduceToNumberProcessor(100);
            var points = reduce.process(data.courseProfile);
            var profileBody = [];
            profileBody.push(new Point(chartArea.x, chartArea.y));
            profileBody.push.apply(profileBody, points);
            profileBody.push(new Point(chartArea.x + chartArea.width, points[points.length - 1].y));
            profileBody.push(new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, points[0].y));
            var bodyPathString = _super.prototype.toPathString.call(this, profileBody) + " Z";
            paper.path(bodyPathString).attr({ fill: "#0A0D14" });
        };
        MountainSilhouetteChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            // var widthMargin: number = surfaceArea.width / 20;
            var heightMargin = surfaceArea.height / 2;
            var mountainArea = surfaceArea.apply(new Margin(10, heightMargin / 2, 10, 10));
            // var headerArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin));
            var chartArea = surfaceArea.apply(new Margin(10, heightMargin, 10, 10));
            var data = new ChartData(profile, result, chartArea);
            var profileExtent = new Extent(data.courseProfile);
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#mountainSilhouetteChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            // var topMargin: number = (profileExtent.minY - chartArea.y);
            // var bottomMargin: number = chartArea.height - (profileExtent.maxY - chartArea.y);
            this.renderBackground(paper, surfaceArea);
            this.renderMountain(paper, new Rectangle(mountainArea.x, mountainArea.y, mountainArea.width, mountainArea.height), profileExtent.maxX, "#EEEEEE");
            this.renderMountain(paper, new Rectangle(mountainArea.x, mountainArea.y + 100, mountainArea.width, mountainArea.height / 2), profileExtent.maxX, "#BBBBBB");
            this.renderMountain(paper, new Rectangle(mountainArea.x, mountainArea.y + 200, mountainArea.width, mountainArea.height / 3), profileExtent.maxX, "#999999");
            this.renderProfile(paper, data, chartArea);
            // this.renderMountain(paper, surfaceArea.apply(new Margin(0, 400, 0, 100)), "#000000");
            var frontProfileHeight = chartArea.height - (profileExtent.maxY - chartArea.y);
            var frontProfileTop = profileExtent.maxY;
            this.renderMountain(paper, new Rectangle(mountainArea.x, frontProfileTop, mountainArea.width, frontProfileHeight), mountainArea.y + mountainArea.height, "#333333");
            this.renderMountain(paper, new Rectangle(mountainArea.x, frontProfileTop, mountainArea.width, frontProfileHeight), mountainArea.y + mountainArea.height, "#222222");
        };
        return MountainSilhouetteChart;
    }(Chart));
    MountainSilhouetteChart.id = "28D33FB8-BEFC-41B3-B947-A0B9B6A811EB";
    ProfileChart.MountainSilhouetteChart = MountainSilhouetteChart;
    var ConnectingDotsChart = (function (_super) {
        __extends(ConnectingDotsChart, _super);
        function ConnectingDotsChart() {
            return _super.call(this, "57B271BD-CA75-42BD-B7FD-A5A0EBEC887F", "Connecting dots", "/App/ProfileChart/Templates/connecting_dots.svg") || this;
        }
        ConnectingDotsChart.prototype.renderPaperCorner = function (paper, location, direction) {
            var points = new Array();
            points.push(location);
            points.push(new Point(location.x + direction.x, location.y));
            points.push(new Point(location.x + direction.x / 100, location.y + direction.y / 100));
            points.push(new Point(location.x, location.y + direction.y));
            points.push(location);
            paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: "#666666", stroke: "none" }); // filter: "url(#penFilter)"
        };
        ConnectingDotsChart.prototype.renderBackgroundGrid = function (paper, paperArea) {
            var gridSize = 32;
            // paper.rect(paperArea.x, paperArea.y, paperArea.width, paperArea.height).attr({ stroke: "#A4A3A3", fill: "none" });
            for (var x = paperArea.x + gridSize; x <= paperArea.x + paperArea.width - gridSize; x += gridSize) {
                paper.line(x, paperArea.y, x, paperArea.y + paperArea.height).attr({ stroke: "#cccccc", strokeWidth: 0.5, fill: "#cccccc" });
            }
            for (var y = paperArea.y + gridSize; y <= paperArea.y + paperArea.height - gridSize; y += gridSize) {
                paper.line(paperArea.x, y, paperArea.x + paperArea.width, y).attr({ stroke: "#cccccc", strokeWidth: 0.5, fill: "#cccccc" });
            }
        };
        ConnectingDotsChart.prototype.renderBackground = function (paper, paperArea) {
            this.renderBackgroundGrid(paper, paperArea);
            var size = paperArea.width / 8;
            this.renderPaperCorner(paper, new Point(paperArea.x, paperArea.y), new Vector(size, size));
            this.renderPaperCorner(paper, new Point(paperArea.x + paperArea.width, paperArea.y), new Vector(-size, size));
            this.renderPaperCorner(paper, new Point(paperArea.x + paperArea.width, paperArea.y + paperArea.height), new Vector(-size, -size));
            this.renderPaperCorner(paper, new Point(paperArea.x, paperArea.y + paperArea.height), new Vector(size, -size));
        };
        ConnectingDotsChart.prototype.renderHeader = function (paper, data, headerArea) {
            var topLeftPoint = new Point(headerArea.x, headerArea.y);
            var topRightPoint = new Point(headerArea.x + headerArea.width, headerArea.y);
            Text.render(paper, data.splits[0].name, topLeftPoint.offset(new Vector(20, 20)), Alignment.leftTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.splits[0].altitude.toFixed(0) + " m", topLeftPoint.offset(new Vector(20, 60)), Alignment.leftTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().name, topRightPoint.offset(new Vector(-20, 20)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " m", topRightPoint.offset(new Vector(-20, 60)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            var athleteDisplayNamePoint = headerArea.getCenter();
            Text.render(paper, data.athlete.displayName, athleteDisplayNamePoint.offset(new Vector(0, -20)), Alignment.centerBottom, { fill: "#1d5f8d", fontSize: "64px" });
            var courseNamePoint = headerArea.getCenter();
            Text.renderInside(paper, data.courseName, courseNamePoint, Alignment.centerTop, 72, headerArea.width, { fill: "#1d5f8d", fontSize: "72px" });
        };
        ConnectingDotsChart.prototype.renderProfile = function (paper, data, profileArea) {
            // paper.rect(profileArea.x, profileArea.y, profileArea.width, profileArea.height).attr({ fill:"none", stroke: "#FF0000" });
            var pipeline = new PointProcessorPipeLine();
            var reduce = new ReduceToNumberProcessor(36);
            pipeline.add(reduce);
            var profile = pipeline.process(data.courseProfile);
            if (profile.length > 0) {
                paper.el("use", { "xlink:href": "#startHere", x: profile[0].x, y: profile[0].y });
                paper.el("use", { "xlink:href": "#finishPen", x: profile[profile.length - 1].x, y: profile[profile.length - 1].y });
                var totalResultTimePoint = profile[profile.length - 1].offset(new Vector(-130, -200));
                Text.render(paper, data.getLastSplit().getTime(), totalResultTimePoint, Alignment.rightMiddle, { fill: "#000000", fontSize: "40px", fontFamily: "Arial", transform: "r45," + totalResultTimePoint.x + "," + totalResultTimePoint.y });
            }
            for (var i = 0; i < profile.length; i++) {
                var dot = paper.circle(profile[i].x, profile[i].y, 5);
                dot.attr({ fill: "#333333" });
                var prev = profile[i - 1];
                var point = profile[i];
                var next = profile[i + 1];
                var textOffset = this.calcTextOffset(prev, point, next, 30);
                var textPoint = profile[i].offset(textOffset);
                var text = paper.text(textPoint.x, textPoint.y, i.toString());
                text.attr({ textAnchor: "middle", dominantBaseline: "middle", fill: "#333333", fontSize: 20 });
            }
            var color = "#a9a9a9";
            paper.path(_super.prototype.toPathString.call(this, profile)).attr({ fill: "none", stroke: color, strokeWidth: 1.7, filter: "url(#penFilter)" }); // filter: "url(#penFilter)"
        };
        ConnectingDotsChart.prototype.renderDistances = function (paper, data, paperArea) {
            var bottomLeftPoint = new Point(paperArea.x, paperArea.y + paperArea.height);
            var bottomRightPoint = new Point(paperArea.x + paperArea.width, paperArea.y + paperArea.height);
            Text.render(paper, data.distanceAxis.format(data.distanceAxis.min, true), bottomLeftPoint.offset(new Vector(20, -20)), Alignment.leftBottom, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.distanceAxis.format(data.distanceAxis.max, true), bottomRightPoint.offset(new Vector(-20, -20)), Alignment.rightBottom, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
        };
        ConnectingDotsChart.prototype.calcTextOffset = function (prev, point, next, distance) {
            var direction;
            if (prev === undefined) {
                direction = Vector.createNormal(point, next);
            }
            else if (next === undefined) {
                direction = Vector.createNormal(prev, point);
            }
            else {
                var firstVector = Vector.createNormalized(prev, point);
                var secondVector = Vector.createNormalized(next, point);
                direction = firstVector.add(secondVector);
            }
            var vector = direction.normalize().scaleTo(distance);
            return vector;
        };
        ConnectingDotsChart.prototype.renderSplits = function (paper, data, profileArea) {
        };
        ConnectingDotsChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var paperArea = surfaceArea.apply(new Margin(10, 10, 10, 10));
            var widthMargin = paperArea.width / 20;
            var chartTopMargin = paperArea.height / 4 - 60;
            var chartBottomMargin = paperArea.height / 16;
            var headerArea = paperArea.apply(new Margin(0, 0, 0, paperArea.height - chartTopMargin));
            var chartArea = paperArea.apply(new Margin(widthMargin, chartTopMargin, widthMargin, chartBottomMargin));
            var profileArea = chartArea.apply(new Margin(0, 200, 0, 0));
            var data = new ChartData(profile, result, profileArea);
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#connectingDotsChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, paperArea);
            this.renderHeader(paper, data, headerArea);
            this.renderDistances(paper, data, paperArea);
            //            paper.rect(chartArea.x, chartArea.y, chartArea.width, chartArea.height).attr({ fill: "none", stroke: "#00FF00" });
            this.renderProfile(paper, data, profileArea);
            // paper.circle(200, 400, 3);
            // paper.el("use", { "xlink:href": "#startHere", x: 200, y: 400 });
            // paper.circle(400, 200, 3);  
            // paper.el("use", { "xlink:href": "#finishPen", x: 400, y: 200 });
        };
        return ConnectingDotsChart;
    }(Chart));
    ProfileChart.ConnectingDotsChart = ConnectingDotsChart;
    var SimplySunshineChart = (function (_super) {
        __extends(SimplySunshineChart, _super);
        function SimplySunshineChart() {
            return _super.call(this, "19930022-DDB3-4CFC-A75E-3E8CC2DEEB04", "Simply Sunshine", "/App/ProfileChart/Templates/simply_sunshine.svg") || this;
        }
        SimplySunshineChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("r(0.5, 0.5, 1)#fff:0-#FBFDFF:30-#EEF8FD:43-#D9F0FB:57-#BBE4F9:71-#fff:25-#95D5F5:86-#67C3F1");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        };
        SimplySunshineChart.prototype.renderGrid = function (paper, data, chartArea) {
            var source = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());
            var transform = new TransformProcessor(source, chartArea);
            paper.rect(chartArea.x, chartArea.y, chartArea.width, chartArea.height).attr({ stroke: "#A4A3A3", fill: "none" });
            for (var altitude = data.altitudeAxis.min; altitude <= data.altitudeAxis.max; altitude = altitude + data.altitudeAxis.gridMinor) {
                var altitudePoint = transform.processPoint(new Point(0, altitude));
                paper.line(chartArea.x, altitudePoint.y, chartArea.x + chartArea.width, altitudePoint.y).attr({ stroke: "#A4A3A3" });
                var altitudeMajor = data.altitudeAxis.major(altitude);
                var altitudeLine = paper.line(chartArea.x - 35, altitudePoint.y, chartArea.x - 20, altitudePoint.y);
                if (altitudeMajor) {
                    altitudeLine.attr({ stroke: "#231F20", strokeWidth: 2 });
                    Text.render(paper, data.altitudeAxis.format(altitude, false), new Point(chartArea.x - 45, altitudePoint.y), Alignment.rightMiddle, { fontSize: "24px", fill: "#515151", fontFamily: "Biko" });
                }
                else {
                    altitudeLine.attr({ stroke: "#A4A3A3", strokeWidth: 1 });
                }
            }
            for (var distance = data.distanceAxis.min; distance <= data.distanceAxis.max; distance = distance + data.distanceAxis.gridMinor) {
                var distancePoint = transform.processPoint(new Point(distance, 0));
                paper.line(distancePoint.x, chartArea.y, distancePoint.x, chartArea.y + chartArea.height).attr({ stroke: "#A4A3A3" });
                var distanceMajor = data.distanceAxis.major(distance);
                var top = chartArea.y + chartArea.height;
                var distanceLine = paper.line(distancePoint.x, top + 10, distancePoint.x, top + 20);
                if (distanceMajor) {
                    distanceLine.attr({ stroke: "#231F20", strokeWidth: 2 });
                    Text.render(paper, data.distanceAxis.format(distance, distance === data.distanceAxis.min), new Point(distancePoint.x, top + 30), Alignment.centerTop, { fontSize: "24px", fill: "#515151", fontFamily: "Biko" });
                }
                else {
                    distanceLine.attr({ stroke: "#A4A3A3", strokeWidth: 1 });
                }
            }
        };
        SimplySunshineChart.prototype.renderProfile = function (paper, data, chartArea) {
            var reduce = new ReduceToNumberProcessor(1000);
            var profile = data.courseProfile;
            profile = reduce.process(profile);
            var profileBody = [];
            profileBody.push(new Point(chartArea.x, profile[0].y));
            profileBody.push.apply(profileBody, profile);
            profileBody.push(new Point(chartArea.x + chartArea.width, profile[profile.length - 1].y));
            profileBody.push(new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, profile[0].y));
            var bodyPathString = _super.prototype.toPathString.call(this, profileBody) + " Z";
            var g = paper.gradient("l(0.5, 1, 0.5, 0)#81F5E0-#56B5FB");
            paper.path(bodyPathString).attr({ fill: g, opacity: 0.8, stroke: "#105A77", strokeWidth: 2, strokeLinejoin: "round" });
        };
        SimplySunshineChart.prototype.renderPlace = function (paper, chartArea, location, name, index) {
            var width = 180;
            var height = 36;
            var signPadding = 5;
            var offsetSignY = 100;
            if (index % 2 === 1) {
                offsetSignY += height + signPadding;
            }
            var signPoint = new Point(location.x, chartArea.y - offsetSignY);
            var text = Text.render(paper, name, signPoint, Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });
            var textSize = text.getBBox();
            var cornerRadius = height / 2;
            width = textSize.width + cornerRadius * 2;
            var signSize = new Offset(width / 2, height / 2);
            var sign = paper.rect(signPoint.x - signSize.width, signPoint.y - signSize.height + 5, width, height, cornerRadius, cornerRadius).attr({ fill: "#56C4CC" });
            text.before(sign);
            var points = [];
            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x, signPoint.y + signSize.height + 14));
            points.push(new Point(signPoint.x + 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));
            text.before(paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: "#56C4CC" }));
            paper.line(signPoint.x, signPoint.y + signSize.height + 14 + 10, location.x, location.y).attr({ fill: "none", stroke: "#000000", strokeWidth: 1, strokeDasharray: "5, 5" });
            ;
        };
        SimplySunshineChart.prototype.renderPlaces = function (paper, data, chartArea) {
            for (var i = 0; i < data.places.length; i++) {
                this.renderPlace(paper, chartArea, data.places[i].point, data.places[i].name, i);
            }
        };
        SimplySunshineChart.prototype.renderClimbs = function (paper, data, chartArea) {
            if (data.track != undefined && data.track.climbs != undefined) {
                for (var i = 0; i < data.track.climbs.length; i++) {
                    var climb = data.track.climbs[i];
                    var segmentPoints = data.profile.getSegmentPoints(climb.start, climb.end);
                    paper.path(_super.prototype.toPathString.call(this, segmentPoints)).attr({ fill: "none", stroke: "#FF0000" });
                }
            }
        };
        SimplySunshineChart.prototype.renderSplits = function (paper, data, chartArea) {
            var source = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());
            var transform = new TransformProcessor(source, chartArea);
            var signHeight = 36;
            var signCornerRadius = signHeight / 2;
            var offset = new Offset(0, -200);
            for (var i = 1; i < data.splits.length; i++) {
                var split = data.splits[i];
                var splitPoint = transform.processCoordinate(split.distance, 0);
                var placeText = Text.render(paper, split.name, new Point(splitPoint.x, chartArea.y + offset.height), Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });
                var placeTextSize = placeText.getBBox();
                var signWidth = placeTextSize.width + signCornerRadius * 2;
                placeText.before(paper.rect(splitPoint.x - signWidth / 2, chartArea.y + offset.height - 18 + 5, signWidth, signHeight, signCornerRadius, signCornerRadius).attr({ fill: "#E03B3B" }));
                var splitTimeY = chartArea.y + offset.height - 24;
                var splitTimeText = Text.render(paper, split.getTime(), new Point(splitPoint.x, splitTimeY), Alignment.centerBottom, { fill: "#333333", fontSize: "24px", fontWeight: "bold", fontFamily: "Arial" });
                var bbox = splitTimeText.getBBox();
                paper.el("use", { "xlink:href": "#stopwatch", x: bbox.x, y: splitTimeY + 12 });
                var radius = 5;
                paper.circle(splitPoint.x, chartArea.y + offset.height + 40, radius).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
                paper.circle(splitPoint.x, chartArea.y + chartArea.height + radius, radius).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
                paper.line(splitPoint.x, chartArea.y + offset.height + 40 + radius, splitPoint.x, chartArea.y + chartArea.height).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
                if (i === data.splits.length - 1) {
                    paper.el("use", { "xlink:href": "#finish_flag", x: splitPoint.x, y: splitTimeY - 18 });
                }
            }
        };
        SimplySunshineChart.prototype.renderSunAndClouds = function (paper) {
            paper.el("use", { "xlink:href": "#sun", x: 240, y: 100 });
            paper.el("use", { "xlink:href": "#clouds", x: 280, y: 130 });
        };
        SimplySunshineChart.prototype.renderHeader = function (paper, data, headerArea) {
            var topCenterPoint = new Point(headerArea.x + headerArea.width / 2, headerArea.y);
            var courseNamePoint = topCenterPoint.offset(new Vector(0, 80));
            var courseNameText = Text.renderInside(paper, data.courseName, courseNamePoint, Alignment.centerBottom, 72, headerArea.width, { fill: "#E03B3B", fontSize: "72px", fontFamily: "Arial" });
            //super.fitTextInto(courseNameText, 72, headerArea.width);
            var bbox = courseNameText.getBBox();
            paper.line(bbox.x, bbox.y + bbox.height, bbox.x + bbox.width, bbox.y + bbox.height).attr({ fill: "none", stroke: "#56C4CC", strokeWidth: 6 });
            Text.render(paper, data.athlete.displayName, courseNamePoint, Alignment.centerTop, { fill: "#E03B3B", fontSize: "64px", fontFamily: "Arial" });
        };
        SimplySunshineChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var widthMargin = surfaceArea.width / 20;
            var heightMargin = surfaceArea.height / 2;
            var headerArea = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin));
            var chartArea = surfaceArea.apply(new Margin(widthMargin * 2, heightMargin, widthMargin, 80));
            // var profileArea: Rectangle = chartArea.apply(new Margin(0, chartArea.height / 4, 0, chartArea.height / 2));
            var data = new ChartData(profile, result, chartArea);
            data.profile.addTransform(new ReduceToNumberProcessor(1000));
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#simplySunshineChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, this.surfaceArea);
            this.renderGrid(paper, data, chartArea);
            this.renderProfile(paper, data, chartArea);
            // this.renderClimbs(paper, data, chartArea);
            this.renderSplits(paper, data, chartArea);
            this.renderPlaces(paper, data, chartArea);
            this.renderSunAndClouds(paper);
            this.renderHeader(paper, data, headerArea);
            // paper.circle(100, 200, 3);
            // paper.el("use", { "xlink:href": "#sun", x: 100, y: 200 });
            // paper.circle(200, 200, 3);
            // paper.el("use", { "xlink:href": "#clouds", x: 200, y: 200 });
            // paper.circle(300, 200, 3);
            // paper.el("use", { "xlink:href": "#finish_flag", x: 300, y: 200 });
            // paper.circle(400, 200, 3);
            // paper.el("use", { "xlink:href": "#stopwatch", x: 400, y: 200 });
        };
        return SimplySunshineChart;
    }(Chart));
    ProfileChart.SimplySunshineChart = SimplySunshineChart;
    var ZodiacChart = (function (_super) {
        __extends(ZodiacChart, _super);
        function ZodiacChart(id, name) {
            return _super.call(this, id, name, "/App/ProfileChart/Templates/zodiac.svg") || this;
        }
        ZodiacChart.prototype.createPaper = function (profile, result, width) {
            var widthMargin = this.surfaceArea.width / 40;
            var headerHeight = 180;
            this.headerArea = this.surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, this.surfaceArea.height - headerHeight));
            this.chartArea = this.surfaceArea.apply(new Margin(widthMargin, headerHeight, widthMargin, widthMargin));
            this.profileArea = this.chartArea.apply(new Margin(0, 50, 0, 200));
            var data = new ChartData(profile, result, this.chartArea);
            var height = (this.surfaceArea.height / this.surfaceArea.width) * width;
            var paper = Snap("#zodiacChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: this.surfaceArea.x + " " + this.surfaceArea.y + " " + this.surfaceArea.width + " " + this.surfaceArea.height });
            this.clearChart();
            return paper;
        };
        ZodiacChart.prototype.getRandomPoint = function (boundary) {
            return new Point(boundary.x + boundary.width * Math.random(), boundary.y + boundary.height * Math.random());
        };
        ZodiacChart.prototype.renderStars = function (paper, surfaceArea, nrOfStars) {
            for (var i = 0; i < nrOfStars; i++) {
                var treeId = "#star" + (Math.floor(Math.random() * 3) + 1);
                var point = this.getRandomPoint(surfaceArea);
                var scale = 0.6 + Math.random() * 0.4;
                var rotate = Math.random() * 45;
                var transform = "scale(" + scale + ", " + scale + ", " + point.x + "," + point.y + ") rotate(" + rotate + ", " + point.x + ", " + point.y + ")";
                paper.el("use", { "xlink:href": treeId, transform: transform, x: point.x / scale, y: point.y / scale });
            }
            paper.el("use", { "xlink:href": "#shootingStar", x: 300, y: 300 });
        };
        ZodiacChart.prototype.renderHeader = function (paper, data, headerArea) {
            var topLeftPoint = new Point(headerArea.x, headerArea.y);
            var topRightPoint = new Point(headerArea.x + headerArea.width, headerArea.y);
            Text.render(paper, data.splits[0].name, topLeftPoint.offset(new Vector(20, 20)), Alignment.leftTop, { fontSize: "32px", fill: "#ffffff", fontFamily: "Arial" });
            Text.render(paper, data.splits[0].altitude.toFixed(0) + " m", topLeftPoint.offset(new Vector(20, 60)), Alignment.leftTop, { fontSize: "32px", fill: "#ffffff", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().name, topRightPoint.offset(new Vector(-20, 20)), Alignment.rightTop, { fontSize: "32px", fill: "#ffffff", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " m", topRightPoint.offset(new Vector(-20, 60)), Alignment.rightTop, { fontSize: "32px", fill: "#ffffff", fontFamily: "Arial" });
            var athleteDisplayNamePoint = headerArea.getCenter();
            Text.render(paper, data.athlete.displayName, athleteDisplayNamePoint.offset(new Vector(0, -20)), Alignment.centerBottom, { fill: "#ffffff", fontSize: "64px" });
            var courseNamePoint = headerArea.getCenter();
            Text.renderInside(paper, data.courseName, courseNamePoint, Alignment.centerTop, 72, headerArea.width, { fill: "#ffffff", fontSize: "72px" });
        };
        ZodiacChart.prototype.renderDistances = function (paper, data, chartArea) {
            var bottomLeftPoint = new Point(chartArea.x, chartArea.y + chartArea.height);
            var bottomRightPoint = new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height);
            Text.render(paper, data.distanceAxis.format(data.distanceAxis.min, true), bottomLeftPoint, Alignment.leftBottom, { fontSize: "32px", fill: "#FFFFFF", fontFamily: "Arial" });
            Text.render(paper, data.distanceAxis.format(data.distanceAxis.max, true), bottomRightPoint, Alignment.rightBottom, { fontSize: "32px", fill: "#FFFFFF", fontFamily: "Arial" });
        };
        ZodiacChart.prototype.renderProfile = function (paper, data, profileArea) {
            //paper.rect(profileArea.x, profileArea.y, profileArea.width, profileArea.height).attr({ fill:"none", stroke: "#FF0000" });
            var profileLineTransform = new ReduceToNumberProcessor(13);
            var profileLine = profileLineTransform.process(data.courseProfile);
            paper.path(_super.prototype.toPathString.call(this, profileLine)).attr({ fill: "none", stroke: "#FFFFFF", strokeWidth: 3, strokeLinejoin: "round" });
            var smallStarsTransform = new ReduceToNumberProcessor(13);
            var smallStarPoints = smallStarsTransform.process(data.courseProfile);
            for (var i = 0; i < smallStarPoints.length; i++) {
                var treeId = "#profileStarSmall";
                var point = smallStarPoints[i];
                //                var scale: number = 0.9 + Math.random() * 0.2;
                var rotate = Math.random() * 45;
                var transform = "rotate(" + rotate + ", " + point.x + ", " + point.y + ")";
                paper.el("use", { "xlink:href": treeId, transform: transform, x: point.x, y: point.y });
            }
            var bigStarsTransform = new ReduceToNumberProcessor(3);
            var bigStarPoints = bigStarsTransform.process(data.courseProfile);
            for (var i = 0; i < bigStarPoints.length; i++) {
                var treeId = "#profileStarBig";
                var point = bigStarPoints[i];
                //                var scale: number = 0.9 + Math.random() * 0.2;
                var rotate = Math.random() * 45;
                var transform = "rotate(" + rotate + ", " + point.x + ", " + point.y + ")";
                paper.el("use", { "xlink:href": treeId, transform: transform, x: point.x, y: point.y });
            }
            var finishPoint = bigStarPoints[bigStarPoints.length - 1];
            paper.el("use", { "xlink:href": "#shootingStarTailFinish", x: finishPoint.x, y: finishPoint.y });
            var finishTextPoint = finishPoint.offset(new Vector(-100, -85));
            Text.render(paper, data.getLastSplit().getTime(), finishTextPoint, Alignment.rightMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial", transform: "r05," + finishTextPoint.x + "," + finishTextPoint.y });
        };
        ZodiacChart.prototype.renderMoon = function (paper, surfaceArea) {
            paper.el("use", { "xlink:href": "#moon", x: 200, y: 200 });
        };
        ZodiacChart.prototype.twinkleStars = function (paper, starArea) {
            if (this.addStar) {
                this.twinklingStars[0].remove();
                this.twinklingStars.shift();
            }
            else {
                this.twinklingStars.push(this.createTwinklingStar(paper, starArea));
            }
            this.addStar = !this.addStar;
        };
        ZodiacChart.prototype.createTwinklingStar = function (paper, starArea) {
            var x = starArea.x + starArea.width * Math.random();
            var y = starArea.y + starArea.height * Math.random();
            return paper.circle(x, y, 2).attr({ fill: "#6C7FB6" });
        };
        ZodiacChart.prototype.startTwinklingStars = function (paper, starArea, nrOfStars) {
            var _this = this;
            this.twinklingStars = new Array();
            for (var i = 1; i < nrOfStars; i++) {
                this.twinklingStars.push(this.createTwinklingStar(paper, starArea));
            }
            setInterval(function () { return _this.twinkleStars(paper, starArea); }, 2000);
        };
        return ZodiacChart;
    }(Chart));
    ProfileChart.ZodiacChart = ZodiacChart;
    var NorthernZodiacChart = (function (_super) {
        __extends(NorthernZodiacChart, _super);
        function NorthernZodiacChart() {
            return _super.call(this, "614483F0-5B42-41B3-939E-24C4BD1660F8", "Northern Zodiac") || this;
        }
        NorthernZodiacChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("r(0.5, 0.5, 0.5)#12389b-#1c234d");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
            //            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: "url(#radialGradient3793)" });
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: "url(#backgroundGradient)" });
        };
        NorthernZodiacChart.prototype.renderTrees = function (paper, surfaceArea, y) {
            paper.el("use", { "xlink:href": "#northernTrees", x: surfaceArea.x, y: y });
        };
        NorthernZodiacChart.prototype.render = function (profile, result, width) {
            var paper = _super.prototype.createPaper.call(this, profile, result, width);
            var data = new ChartData(profile, result, this.profileArea);
            this.renderBackground(paper, this.surfaceArea);
            _super.prototype.renderStars.call(this, paper, this.surfaceArea, 30);
            _super.prototype.renderHeader.call(this, paper, data, this.headerArea);
            _super.prototype.renderProfile.call(this, paper, data, this.profileArea);
            this.renderTrees(paper, this.surfaceArea, 600);
            _super.prototype.renderDistances.call(this, paper, data, this.chartArea);
            var starArea = this.chartArea.apply(new Margin(0, 0, 0, 200));
            _super.prototype.startTwinklingStars.call(this, paper, starArea, 21);
            //paper.circle(100, 200, 3).attr({ fill: "#FF0000"});
            //paper.el("use", { "xlink:href": "#star1", x: 100, y: 200 });
            //paper.circle(200, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#star2", x: 200, y: 200 });
            //paper.circle(300, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#star3", x: 300, y: 200 });
            //paper.circle(400, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#profileStarSmall", x: 400, y: 200 });
            //paper.circle(500, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#profileStarBig", x: 500, y: 200 });
            //paper.circle(600, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#shootingStar", x: 600, y: 200 });
            //paper.circle(700, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#shootingStarTailFinish", x: 700, y: 200 });
            //paper.circle(100, 400, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#forest", x: 100, y: 400 });
        };
        return NorthernZodiacChart;
    }(ZodiacChart));
    ProfileChart.NorthernZodiacChart = NorthernZodiacChart;
    var SouthernZodiacChart = (function (_super) {
        __extends(SouthernZodiacChart, _super);
        function SouthernZodiacChart() {
            return _super.call(this, "507AF72C-5678-4AFC-AB43-AB7DC34D904E", "Southern Zodiac") || this;
        }
        SouthernZodiacChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("r(0.5, 0.5, 0.5)#6e4c4c-#44234d");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: "url(#backgroundGradient)" });
        };
        SouthernZodiacChart.prototype.renderTrees = function (paper, surfaceArea, y) {
            paper.el("use", { "xlink:href": "#southernTrees", x: surfaceArea.x, y: y });
        };
        SouthernZodiacChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var widthMargin = surfaceArea.width / 40;
            //            var heightMargin: number = surfaceArea.height / 2;
            var headerHeight = 180;
            var headerArea = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, surfaceArea.height - headerHeight));
            var chartArea = surfaceArea.apply(new Margin(widthMargin, headerHeight, widthMargin, widthMargin));
            var profileArea = chartArea.apply(new Margin(0, 0, 0, 0));
            var data = new ChartData(profile, result, chartArea);
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#zodiacChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, this.surfaceArea);
            _super.prototype.renderStars.call(this, paper, this.surfaceArea, 30);
            _super.prototype.renderHeader.call(this, paper, data, headerArea);
            _super.prototype.renderProfile.call(this, paper, data, profileArea);
            this.renderTrees(paper, surfaceArea, 600);
            _super.prototype.renderDistances.call(this, paper, data, chartArea);
            //super.renderMoon(paper, this.surfaceArea);
            //paper.circle(100, 200, 3).attr({ fill: "#FF0000"});
            //paper.el("use", { "xlink:href": "#star1", x: 100, y: 200 });
            //paper.circle(200, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#star2", x: 200, y: 200 });
            //paper.circle(300, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#star3", x: 300, y: 200 });
            //paper.circle(400, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#profileStarSmall", x: 400, y: 200 });
            //paper.circle(500, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#profileStarBig", x: 500, y: 200 });
            //paper.circle(600, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#shootingStar", x: 600, y: 200 });
            //paper.circle(700, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#shootingStarTailFinish", x: 700, y: 200 });
            //paper.circle(800, 200, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#moon", x: 800, y: 200 });
            //paper.circle(100, 400, 3).attr({ fill: "#FF0000" });
            //paper.el("use", { "xlink:href": "#southernTree1", x: 100, y: 400 });
        };
        return SouthernZodiacChart;
    }(ZodiacChart));
    ProfileChart.SouthernZodiacChart = SouthernZodiacChart;
    var GiroItaliaChart = (function (_super) {
        __extends(GiroItaliaChart, _super);
        function GiroItaliaChart() {
            return _super.call(this, "E6C5D286-BF69-4FD0-A6DE-F46ACC53F011", "Giro d'Italia", "/App/ProfileChart/Templates/giro_italia.svg") || this;
        }
        GiroItaliaChart.prototype.renderBackground = function (paper, surfaceArea) {
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: '#FFFFFF' });
        };
        GiroItaliaChart.prototype.renderRulerScaleSegment = function (paper, from, to, rulerHeight, color) {
            var points = [];
            points.push(from);
            points.push(to);
            points.push(to.offset(rulerHeight));
            points.push(from.offset(rulerHeight));
            paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: color, stroke: "#000000", strokeWidth: 2 });
        };
        GiroItaliaChart.prototype.renderDistanceRuler = function (paper, data, chartArea, transform) {
            //var source: Rectangle = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());
            var rulerHeight = new Vector(0, -12);
            var lastMajorDistancePoint = null;
            var even = true;
            var color = "#000000";
            for (var distance = data.distanceAxis.min; distance <= data.distanceAxis.max; distance = distance + data.distanceAxis.gridMinor) {
                var distancePoint = transform.processPoint(data.transform.processPoint(new Point(distance, data.altitudeAxis.min)));
                var distanceMajor = data.distanceAxis.major(distance);
                if (distanceMajor) {
                    if (lastMajorDistancePoint != undefined) {
                        color = even ? "#000000" : "#FFFFFF";
                        this.renderRulerScaleSegment(paper, lastMajorDistancePoint, distancePoint, rulerHeight, color);
                        even = !even;
                    }
                    Text.render(paper, data.distanceAxis.format(distance, false), new Point(distancePoint.x, distancePoint.y + 8), Alignment.centerTop, { fontSize: "24px", fill: "#515151", fontFamily: "Biko" });
                    lastMajorDistancePoint = distancePoint;
                }
                if (lastMajorDistancePoint != undefined) {
                    var endPoint = transform.processPoint(data.transform.processPoint(new Point(data.distanceAxis.max, data.altitudeAxis.min)));
                    color = even ? "#000000" : "#FFFFFF";
                    this.renderRulerScaleSegment(paper, lastMajorDistancePoint, endPoint, rulerHeight, color);
                }
            }
        };
        GiroItaliaChart.prototype.renderGround = function (paper, profile, offset, origo) {
            var color = "#A2A7AF";
            var ascendingColor = "#A2A7AF";
            var descandingColor = "#999999";
            var points = [];
            points.push(origo);
            points.push(profile[0]);
            points.push(profile[0].offset(offset));
            points.push(origo.offset(offset));
            paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: ascendingColor });
            for (var i = 1; i < profile.length; i++) {
                points = [];
                points.push(profile[i - 1]);
                points.push(profile[i - 1].offset(offset));
                points.push(profile[i].offset(offset));
                points.push(profile[i]);
                points.push(profile[i - 1]);
                if (profile[i - 1].y > profile[i].y) {
                    color = ascendingColor;
                }
                else {
                    color = descandingColor;
                }
                var pathString = _super.prototype.toPathString.call(this, points) + " Z";
                paper.path(pathString).attr({ fill: color });
            }
        };
        GiroItaliaChart.prototype.renderLine = function (paper, from, to) {
            return paper.line(from.x, from.y, to.x, to.y);
        };
        GiroItaliaChart.prototype.renderProfile = function (paper, data, chartArea, frontTransform) {
            var frontOffset = new Vector(24, 14);
            var reduce = new ReduceToNumberProcessor(100);
            var skew = new SkewProcessor(new Vector(10, -1).normalize());
            var offset = new OffsetProcessor(frontOffset);
            var profile = reduce.process(data.courseProfile);
            var backProfile = skew.process(profile);
            var backOrigo = skew.processPoint(new Point(chartArea.x, chartArea.y + chartArea.height));
            var frontProfile = offset.process(skew.process(profile));
            paper.path(_super.prototype.toPathString.call(this, backProfile)).attr({ fill: 'none', stroke: "#000000", strokeWidth: 9, strokeLinejoin: "round" });
            this.renderGround(paper, backProfile, frontOffset, backOrigo);
            var lineAttr = { fill: 'none', stroke: "#000000", strokeWidth: 7 };
            //this.drawLine(paper, backProfile[backProfile.length - 1], frontProfile[frontProfile.length - 1]).attr(lineAttr);
            this.renderLine(paper, backProfile[0], frontProfile[0]).attr({ fill: 'none', stroke: "#000000", strokeWidth: 5 });
            this.renderLine(paper, backProfile[0], frontProfile[0]).attr({ fill: 'none', stroke: "#AAAAAA", strokeWidth: 2 });
            this.renderLine(paper, backProfile[0], backOrigo).attr(lineAttr);
            this.renderLine(paper, backOrigo, backOrigo.offset(frontOffset)).attr({ fill: 'none', stroke: "#000000", strokeWidth: 5 });
            var profileBody = [];
            profileBody.push(new Point(chartArea.x, profile[0].y));
            profileBody.push.apply(profileBody, profile);
            profileBody.push(new Point(chartArea.x + chartArea.width, profile[profile.length - 1].y));
            profileBody.push(new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, profile[0].y));
            var frontProfileBody = offset.process(skew.process(profileBody));
            var g = paper.gradient("l(0.5, 1, 0.5, 0)#FCFDED:25-#D7E8BE:50-#EEF8FD:75-#EDCAA0:100-#F8FAF9");
            g.transform("r-22 0 0");
            var frontBodyPathString = _super.prototype.toPathString.call(this, frontProfileBody) + " Z";
            paper.path(frontBodyPathString).attr({ fill: g, stroke: "#000000", strokeWidth: 4, strokeLinejoin: "round" });
            var frontProfilePathString = _super.prototype.toPathString.call(this, frontProfile);
            paper.path(frontProfilePathString).attr({ fill: 'none', stroke: "#000000", strokeWidth: 9, strokeLinejoin: "round" });
            paper.path(frontProfilePathString).attr({ fill: 'none', stroke: "#CC2D3C", strokeWidth: 5, strokeLinejoin: "round" });
        };
        GiroItaliaChart.prototype.renderPlace = function (paper, chartArea, location, name, index) {
            var width = 180;
            var height = 36;
            var signPadding = 5;
            var offsetSignY = 100;
            if (index % 2 === 1) {
                offsetSignY += height + signPadding;
            }
            var signSize = new Offset(width / 2, height / 2);
            var signPoint = new Point(location.x, chartArea.y - offsetSignY);
            paper.rect(signPoint.x - signSize.width, signPoint.y - signSize.height + 5, width, height, signSize.height, signSize.height).attr({ fill: "#56C4CC" });
            var points = [];
            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x, signPoint.y + signSize.height + 14));
            points.push(new Point(signPoint.x + 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));
            paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: "#56C4CC" });
            Text.render(paper, name, signPoint, Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });
            paper.line(signPoint.x, signPoint.y + signSize.height + 14 + 10, location.x, location.y).attr({ fill: "none", stroke: "#000000", strokeWidth: 1, strokeDasharray: "5, 5" });
            ;
        };
        GiroItaliaChart.prototype.renderPlaces = function (paper, data, chartArea) {
            for (var i = 0; i < data.places.length; i++) {
                this.renderPlace(paper, chartArea, data.places[i].point, data.places[i].name, i);
            }
        };
        GiroItaliaChart.prototype.renderStartFinish = function (paper, data, split, textAlignment, backTransform, iconName) {
            var lineOffset = new Vector(0, -550);
            var textOffset = new Vector(60, -25);
            if (textAlignment.horizontal === HorizontalAlignment.Right)
                textOffset.x = -textOffset.x;
            var basePoint = backTransform.processPoint(split.point);
            var topPoint = backTransform.processPoint(data.transform.processPoint(new Point(split.distance, data.altitudeAxis.min))).offset(lineOffset);
            this.renderLine(paper, basePoint, topPoint).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
            var text = Text.render(paper, split.altitude.toFixed(0) + " - " + split.name.toUpperCase(), topPoint.offset(textOffset), textAlignment, { fontSize: "48px", fill: "#000000", fontFamily: "Arial", fontWeight: "bold" });
            text.transform("r-5 " + topPoint.x + " " + topPoint.y);
            paper.el("use", { "xlink:href": iconName, x: topPoint.x, y: topPoint.y });
        };
        GiroItaliaChart.prototype.renderTotalTime = function (paper, data, split, backTransform) {
            var lineOffset = new Vector(0, -550);
            var textOffset = new Vector(60, -25);
            var timePoint = backTransform.processPoint(data.transform.processPoint(new Point(split.distance / 2, data.altitudeAxis.min))).offset(lineOffset);
            var text = Text.render(paper, split.getTime(), timePoint.offset(textOffset), Alignment.centerBottom, { fontSize: "48px", fill: "#000000", fontFamily: "Arial", fontWeight: "bold" });
            text.transform("r-5 " + timePoint.x + " " + timePoint.y);
            //paper.el("use", { "xlink:href": iconName, x: topPoint.x, y: topPoint.y });
        };
        GiroItaliaChart.prototype.renderDistance = function (paper, data, split, frontTransform, bold) {
            var topPoint = frontTransform.processPoint(split.point);
            var basePoint = frontTransform.processPoint(data.transform.processPoint(new Point(split.distance, data.altitudeAxis.min))).offset(new Vector(0, 30));
            var textPoint = basePoint.offset(new Vector(0, 5));
            this.renderLine(paper, basePoint, topPoint).attr({ fill: "none", stroke: "#000000", strokeWidth: 3 });
            var fontWeight = bold ? "bold" : "normal";
            var text = Text.render(paper, data.distanceAxis.format(split.distance, false), textPoint, Alignment.rightBottom, { fontSize: "24px", fill: "#000000", fontFamily: "Arial", fontWeight: fontWeight });
            text.transform("r-90 " + textPoint.x + " " + textPoint.y);
        };
        GiroItaliaChart.prototype.renderSplits = function (paper, data, chartArea, backTransform, frontTransform) {
            this.renderStartFinish(paper, data, data.getFirstSplit(), Alignment.leftBottom, backTransform, "#start_icon");
            this.renderDistance(paper, data, data.getFirstSplit(), frontTransform, true);
            this.renderStartFinish(paper, data, data.getLastSplit(), Alignment.rightBottom, backTransform, "#finish_icon");
            this.renderDistance(paper, data, data.getLastSplit(), frontTransform, true);
            this.renderTotalTime(paper, data, data.getLastSplit(), backTransform);
            //var direction: Vector = new Vector(10, -1);
            //var directionPoint = startTopPoint.offset(direction.scaleToX(1600));
            //this.renderLine(paper, startTopPoint, directionPoint).attr({ fill: "none", stroke: "#000000", strokeWidth: 1 });
            //this.renderLine(paper, startBasePoint, startTopPoint).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
            //var startText: Snap.Element = Text.render(paper, data.getFirstSplit().altitude.toFixed(0) + " - " + data.getFirstSplit().name.toUpperCase(), startTopPoint.offset(textOffset), Alignment.leftBottom, { fontSize: "48px", fill: "#000000", fontFamily: "Arial" });
            //startText.transform("r-5 " + startTopPoint.x + " " + startTopPoint.y);
            //Text.render(paper, data.getLastSplit().name, topRightPoint.offset(new Vector(-20, 20)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            //Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " m", topRightPoint.offset(new Vector(-20, 60)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            var offset = new Offset(0, -200);
            //for (var i: number = 1; i < data.splits.length; i++) {
            //    var split: ChartSplit = data.splits[i];
            //    var splitPoint: Point = backTransform.processPoint(new Point(split.distance, 0));
            //    paper.rect(splitPoint.x - 90, chartArea.y + offset.height - 18 + 5, 180, 36, 18, 18).attr({ fill: "#E03B3B" });
            //    Text.render(paper, split.name, new Point(splitPoint.x, chartArea.y + offset.height), Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });
            //    var splitTimeY: number = chartArea.y + offset.height - 24;
            //    var splitTimeText: Snap.Element = Text.render(paper, split.getTime(), new Point(splitPoint.x, splitTimeY), Alignment.centerBottom, { fill: "#333333", fontSize: "24px", fontWeight: "bold", fontFamily: "Arial" });
            //    var bbox: Snap.BBox = splitTimeText.getBBox();
            //    paper.el("use", { "xlink:href": "#stopwatch", x: bbox.x, y: splitTimeY + 12 });
            //    var radius: number = 5;
            //    paper.circle(splitPoint.x, chartArea.y + offset.height + 40, radius).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
            //    paper.circle(splitPoint.x, chartArea.y + chartArea.height + radius, radius).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
            //    paper.line(splitPoint.x, chartArea.y + offset.height + 40 + radius, splitPoint.x, chartArea.y + chartArea.height).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
            //}
            //var finishBasePoint = backTransform.processPoint(data.getLastSplit().point);
            //var finishTopPoint = finishBasePoint.offset(lineOffset);
            //this.renderLine(paper, finishBasePoint, finishTopPoint).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
            //var finishText: Snap.Element = Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " - " + data.getLastSplit().name.toUpperCase(), finishTopPoint.offset(textOffset), Alignment.rightBottom, { fontSize: "48px", fill: "#000000", fontFamily: "Arial" });
            //finishText.transform("r-5 " + finishTopPoint.x + " " + finishTopPoint.y);
        };
        GiroItaliaChart.prototype.renderSunAndClouds = function (paper) {
            paper.el("use", { "xlink:href": "#sun", x: 240, y: 100 });
            paper.el("use", { "xlink:href": "#clouds", x: 280, y: 130 });
        };
        GiroItaliaChart.prototype.renderHeader = function (paper, data, headerArea) {
            var topCenterPoint = new Point(headerArea.x + headerArea.width / 2, headerArea.y);
            var courseNamePoint = topCenterPoint.offset(new Vector(0, 80));
            Text.renderInside(paper, data.courseName, courseNamePoint, Alignment.centerBottom, 72, headerArea.width, { fill: "#000000", fontSize: "72px", fontFamily: "Arial" });
            //    courseNameText.transform("r-5 " + courseNamePoint.x + " " + courseNamePoint.y);
            //            var bbox: Snap.BBox = courseNameText.getBBox();
            //            paper.line(bbox.x, bbox.y + bbox.height, bbox.x + bbox.width, bbox.y + bbox.height).attr({ fill: "none", stroke: "#56C4CC", strokeWidth: 6 });
            var athleteNameText = Text.render(paper, data.athlete.displayName, courseNamePoint, Alignment.centerTop, { fill: "#000000", fontSize: "64px", fontFamily: "Arial" });
            //athleteNameText.transform("r-5 " + courseNamePoint.x + " " + courseNamePoint.y);
        };
        GiroItaliaChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var widthMargin = surfaceArea.width / 20;
            var heightMargin = surfaceArea.height / 2;
            var headerArea = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin));
            var chartArea = surfaceArea.apply(new Margin(widthMargin * 2, heightMargin, widthMargin, 80));
            // var profileArea: Rectangle = chartArea.apply(new Margin(0, chartArea.height / 4, 0, chartArea.height / 2));
            var data = new ChartData(profile, result, chartArea);
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#giroItaliaChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, this.surfaceArea);
            var frontOffset = new Vector(24, 14);
            var reduce = new ReduceToNumberProcessor(100);
            var skew = new SkewProcessor(new Vector(10, -1).normalize());
            var offset = new OffsetProcessor(frontOffset);
            var backTransform = new PointProcessorPipeLine();
            //            backTransform.add(data.transform);
            backTransform.add(skew);
            var frontTransform = new PointProcessorPipeLine();
            //frontTransform.add(data.transform);
            frontTransform.add(skew);
            frontTransform.add(offset);
            this.renderProfile(paper, data, chartArea, frontTransform);
            this.renderDistanceRuler(paper, data, chartArea, frontTransform);
            this.renderSplits(paper, data, chartArea, backTransform, frontTransform);
            //this.renderPlaces(paper, data, chartArea);
            //this.renderSunAndClouds(paper);
            this.renderHeader(paper, data, headerArea);
            //paper.circle(100, 200, 3);
            //paper.el("use", { "xlink:href": "#start_icon", x: 100, y: 200 });
            // paper.circle(200, 200, 3);
            // paper.el("use", { "xlink:href": "#finish_icon", x: 200, y: 200 });
            //paper.circle(300, 200, 3);
            //paper.el("use", { "xlink:href": "#highest_point", x: 300, y: 200 });
            // paper.circle(400, 200, 3);
            // paper.el("use", { "xlink:href": "#stopwatch", x: 400, y: 200 });
        };
        return GiroItaliaChart;
    }(Chart));
    ProfileChart.GiroItaliaChart = GiroItaliaChart;
    var ForestChart = (function (_super) {
        __extends(ForestChart, _super);
        function ForestChart(settings) {
            var _this = _super.call(this, settings.chartId, settings.name, "/App/ProfileChart/Templates/forest.svg") || this;
            _this.settings = settings;
            _this.maxVisibleSplitResults = 7;
            _this.finishResultTimeOffset = new Vector(0, -210);
            _this.finishResultPositionOffset = new Vector(0, -210);
            _this.splitResultTimeOffset = new Vector(-50, -155);
            _this.splitResultPositionOffset = new Vector(140, -160);
            _this.splitResultLabelOffset = new Vector(-30, -120);
            _this.trackOffset = new Vector(0, 150);
            _this.stopwatchWidth = 20;
            return _this;
        }
        ForestChart.prototype.renderBackground = function (paper, surfaceArea) {
            var g = paper.gradient("l(0, 1, 1, 0)#29ABE2-#FFFFFF");
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        };
        ForestChart.prototype.renderHeader = function (paper, data, area) {
            var topLeftPoint = new Point(data.getFirstSplit().point.x, area.y);
            var topRightPoint = new Point(data.getLastSplit().point.x, area.y);
            Text.render(paper, data.splits[0].altitude.toFixed(0) + "m " + data.splits[0].name, topLeftPoint, Alignment.leftTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().altitude.toFixed(0) + "m " + data.getLastSplit().name, topRightPoint, Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, "+" + data.ascending.toFixed(0) + "m ", topRightPoint.offset(new Vector(0, 40)), Alignment.rightTop, { fontSize: "20px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, "-" + data.descending.toFixed(0) + "m ", topRightPoint.offset(new Vector(0, 60)), Alignment.rightTop, { fontSize: "20px", fill: "#676868", fontFamily: "Arial" });
            var topCenterPoint = new Point(area.x + area.width / 2, area.y);
            // var courseNameText = paper.text(textPoint.x, textPoint.y, data.courseName);
            // courseNameText.attr({ fontSize: "48px", fill: "#2ba7de", fontFamily: "Arial" });
            var courseNamePoint = topCenterPoint.offset(new Vector(0, 60));
            // var courseNameShadow = this.renderText(paper, data.courseName, courseNamePoint.offset(new Vector(-2, 2)), Alignment.centerTop, { fontSize: "48px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.courseName, courseNamePoint, Alignment.centerTop, { fontSize: "48px", fill: "#676868", fontFamily: "Arial" });
            var name = data.athlete.displayName;
            var resultNamePoint = topCenterPoint.offset(new Vector(0, 5));
            // var personNameShadow = this.renderText(paper, name, personNamePoint.offset(new Vector(-2, 2)), Alignment.centerTop, { fontSize: "56px", fill: "#676868", fontFamily: "Arial", opacity: 0.75 });
            Text.render(paper, name, resultNamePoint, Alignment.centerTop, { fontSize: "56px", fill: "#676868", fontFamily: "Arial" });
            if (this.settings.renderPersonProfile) {
                // var offsetLeftPersonName = this.calcAlignmentVector(personName, HorizontalAlignment.Center, VerticalAlignment.Top);
                // var resultPositionPoint = resultNamePoint.offset(offsetLeftPersonName).offset(marginLeft);
                // var resultPosition = this.renderText(paper, data.person.startNumber, resultPositionPoint, Alignment.rightBottom, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
                // var offsetRightPersonName = offsetLeftPersonName.scale(-1, 1);
                // var resultClubPoint = resultNamePoint.offset(offsetRightPersonName).offset(marginRight);
                // var resultClub = this.renderText(paper, data.person.club, resultClubPoint, Alignment.leftBottom, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            }
            var resultTimePoint = topCenterPoint.offset(new Vector(0, 120));
            Text.render(paper, new ElapsedTime(data.elapsedSeconds).toString(), resultTimePoint, Alignment.centerTop, { fontSize: "64px", fill: "#676868", fontFamily: "Arial" });
        };
        ForestChart.prototype.renderProfile = function (paper, data, surfaceArea, profileArea) {
            // var reduce = new ReduceProcessor(2, 1);
            var reduce = new ReduceToNumberProcessor(30);
            var profile = data.courseProfile;
            profile = reduce.process(profile);
            var offset = new Vector(5, -15);
            this.renderGround(paper, profile, offset, surfaceArea);
            // paper.path(super.toPathString(profile)).attr({ fill: "none", stroke: color, strokeWidth: 1 });
            // paper.path(super.toPathString(courseProfile)).attr({ fill: "none", stroke: "#FF0000", strokeWidth: 1 });        
            // from #bfdbf2 to #0294d8
            var profileBody = [];
            profileBody.push(new Point(surfaceArea.x, profile[0].y));
            profileBody.push.apply(profileBody, profile);
            profileBody.push(new Point(surfaceArea.x + surfaceArea.width, profile[profile.length - 1].y));
            profileBody.push(new Point(surfaceArea.x + surfaceArea.width, surfaceArea.y + surfaceArea.height));
            profileBody.push(new Point(surfaceArea.x, surfaceArea.y + surfaceArea.height));
            profileBody.push(new Point(surfaceArea.x, profile[0].y));
            var bodyPathString = _super.prototype.toPathString.call(this, profileBody) + " Z";
            var g = paper.gradient("l(0, 0.5, 1, 0.5)#bfdbf2-#0294d8");
            paper.path(bodyPathString).attr({ fill: g });
            var totalLength = 0.0;
            for (var i = 1; i < profile.length; i++) {
                totalLength += Vector.create(profile[i - 1], profile[i]).getLength();
            }
            for (var i = 1; i < profile.length; i++) {
                var length = Vector.create(profile[i - 1], profile[i]).getLength();
                this.renderTrees(paper, profileArea, profile[i - 1], profile[i], offset, Math.floor(length * 100 / totalLength));
            }
            this.renderOwlTree(paper, profile);
            if (this.settings.renderSkierAndTrack) {
                this.renderSkierAndTrack(paper, profile);
            }
            paper.el("use", { "xlink:href": "#finish_flag", x: profile[profile.length - 1].x, y: profile[profile.length - 1].y });
            // paper.rect(profileArea.x, profileArea.y, profileArea.width, profileArea.height).attr({ fill: "none", stroke: "#FF0000" });
        };
        ForestChart.prototype.renderSkierAndTrack = function (paper, profile) {
            var trackReduce = new ReduceToNumberProcessor(10);
            var offseter = new OffsetProcessor(this.trackOffset);
            var trackPoints = offseter.process(trackReduce.process(profile));
            trackPoints.unshift(offseter.processPoint(profile[0]));
            trackPoints.push(offseter.processPoint(profile[profile.length - 1]));
            // paper.path(super.toPathString(trackPoints)).attr({ fill: "none", stroke: "#000000", strokeWidth: 1 });
            paper.path(_super.prototype.createCurveThroughPath.call(this, trackPoints)).attr({ fill: "none", stroke: "#000000", strokeWidth: 1 });
            offseter = new OffsetProcessor(new Vector(0, -2));
            trackPoints = offseter.process(trackPoints);
            paper.path(_super.prototype.createCurveThroughPath.call(this, trackPoints)).attr({ fill: "none", stroke: "#FFFFFF", strokeWidth: 2 });
            paper.el("use", { "xlink:href": "#skier", x: trackPoints[0].x, y: trackPoints[0].y });
        };
        ForestChart.prototype.renderGround = function (paper, profile, offset, surfaceArea) {
            var color = "#666666";
            var ascendingColor = "#808080";
            var descandingColor = "#999999";
            var points = [];
            points.push(new Point(surfaceArea.x, profile[0].y));
            points.push(new Point(surfaceArea.x, profile[0].y + offset.y));
            points.push(profile[0].offset(offset));
            points.push(profile[0]);
            points.push(new Point(surfaceArea.x, profile[0].y));
            paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: descandingColor });
            for (var i = 1; i < profile.length; i++) {
                points = [];
                points.push(profile[i - 1]);
                points.push(profile[i - 1].offset(offset));
                points.push(profile[i].offset(offset));
                points.push(profile[i]);
                points.push(profile[i - 1]);
                if (profile[i - 1].y > profile[i].y) {
                    color = ascendingColor;
                }
                else {
                    color = descandingColor;
                }
                var pathString = _super.prototype.toPathString.call(this, points) + " Z";
                paper.path(pathString).attr({ fill: color });
            }
            points.push(profile[profile.length - 1]);
            points.push(new Point(surfaceArea.x + surfaceArea.width, profile[profile.length - 1].y));
            points.push(new Point(surfaceArea.x + surfaceArea.width, profile[profile.length - 1].y + offset.y));
            points.push(profile[profile.length - 1].offset(offset));
            points.push(profile[profile.length - 1]);
            paper.path(_super.prototype.toPathString.call(this, points) + " Z").attr({ fill: descandingColor });
        };
        ForestChart.prototype.renderOwlTree = function (paper, profile) {
            var _this = this;
            var colors = ["#588427", "#6a992f", "#48711e", "#31550e", "#3d6317", "#1f4100", "#395f15", "#3a6015", "#32560f", "#77a935", "#365f16", "#173900"];
            var segment = Math.floor(Math.random() * (profile.length - 1));
            var from = profile[segment];
            var to = profile[segment];
            var point = this.getRandomTreePoint(from, to, new Vector(0, 0));
            var color = colors[Math.floor(Math.random() * colors.length)];
            paper.el("use", { "xlink:href": "#tree4", x: point.x, y: point.y, fill: color });
            var owl = paper.el("use", { "xlink:href": "#owl", x: point.x, y: point.y, opacity: 0 });
            var eyes = paper.el("use", { "xlink:href": "#owl_eyes", x: point.x, y: point.y });
            eyes.hover(function (event) { _this.showElement(owl); }, function (event) { _this.hideElement(owl); });
        };
        ForestChart.prototype.renderTrees = function (paper, profileArea, from, to, offset, nrOfTrees) {
            var colors = ["#588427", "#6a992f", "#48711e", "#31550e", "#3d6317", "#1f4100", "#395f15", "#3a6015", "#32560f", "#77a935", "#365f16", "#173900"];
            for (var i = 0; i < nrOfTrees; i++) {
                var treeId = "#tree" + (Math.floor(Math.random() * 4) + 1);
                var point = this.getRandomTreePoint(from, to, offset);
                var color = colors[Math.floor(Math.random() * colors.length)];
                var elevationScale = 0.6 + (point.y - profileArea.y) / profileArea.height * 0.6;
                var scaleX = (0.6 + Math.random() * 0.4) * elevationScale;
                var scaleY = (0.7 + Math.random() * 0.3) * elevationScale;
                var transform = "scale(" + scaleX + ", " + scaleY + ", " + point.x + "," + point.y + ")"; // translate(" + (point.x * -1/scaleX) + ", " + (point.y * 1/scaleY) + ")";
                paper.el("use", { "xlink:href": treeId, transform: transform, x: point.x / scaleX, y: point.y / scaleY, fill: color });
            }
        };
        ForestChart.prototype.getRandomTreePoint = function (from, to, offset) {
            var firstvector = Vector.create(from, to).scaleTo(Math.random());
            var secondvector = offset.scaleTo(Math.random());
            var vector = firstvector.add(secondvector);
            return from.offset(vector);
        };
        ForestChart.prototype.showEventHandler = function (event) {
            return function () {
                // var el = <Snap.Element>event.srcElement;
                // el.attr({ opacity: "1" });
            };
        };
        ForestChart.prototype.hideEventHandler = function (event) {
            return function () {
                // el.animate({ opacity: "0" }, 1000);
            };
        };
        ForestChart.prototype.showElement = function (el) {
            el.attr({ opacity: "1" });
        };
        ForestChart.prototype.hideElement = function (el) {
            el.animate({ opacity: "0" }, 1000);
        };
        ForestChart.prototype.show = function (el) {
            return function () {
                el.attr({ opacity: "1" });
            };
        };
        ForestChart.prototype.hide = function (el) {
            return function () {
                el.animate({ opacity: "0" }, 1000);
            };
        };
        ForestChart.prototype.renderSplits = function (paper, data, chartArea) {
            // var start = new Point(data.splits[0].point.x, chartArea.y + chartArea.height);
            // var finish = new Point(data.splits[data.splits.length - 1].point.x, chartArea.y + chartArea.height)
            paper.line(data.splits[0].point.x, chartArea.y + chartArea.height, data.splits[data.splits.length - 1].point.x, chartArea.y + chartArea.height).attr({ stroke: "#555555", strokeWidth: 1 });
            var rigthBottom = new Vector(6, 0);
            var top = new Vector(0, -10);
            var leftBottom = new Vector(-6, 0);
            var distanceFontFalily = "Arial";
            for (var i = 0; i < data.splits.length; i++) {
                var split = data.splits[i];
                paper.line(data.splits[i].point.x, data.splits[i].point.y, data.splits[i].point.x, chartArea.y + chartArea.height).attr({ stroke: "#555555", strokeWidth: 1, opacity: 0.65 });
                var circle = null;
                if (i > 0) {
                    circle = paper.circle(data.splits[i].point.x, data.splits[i].point.y, 6).attr({ fill: "#FFFFFF", stroke: "#000000", strokeWidth: 1 });
                }
                var points = [];
                var centerBottom = new Point(data.splits[i].point.x, chartArea.y + chartArea.height);
                points.push(centerBottom);
                points.push(centerBottom.offset(rigthBottom));
                points.push(centerBottom.offset(top));
                points.push(centerBottom.offset(leftBottom));
                points.push(centerBottom);
                paper.path(_super.prototype.toPathString.call(this, points) + "Z").attr({ fill: "#FFFFFF", stroke: "#555555", strokeWidth: 1 });
                var s = paper.text(centerBottom.x, centerBottom.y, data.distanceAxis.format(split.distance, i === data.splits.length - 1)).attr({ fontFamily: distanceFontFalily, textAnchor: "middle", fontSize: 26, fontStyle: "bold", fill: "#676868" });
                var bbox = s.getBBox();
                s.attr({ y: centerBottom.y + bbox.height });
                if (i > 0 && i < data.splits.length - 1) {
                    var splitResultSymbol = paper.el("use", { id: "split" + i, "xlink:href": "#result_split", x: data.splits[i].point.x, y: data.splits[i].point.y });
                    var splitResultTimePoint = data.splits[i].point.offset(this.splitResultTimeOffset);
                    var splitTime = paper.text(splitResultTimePoint.x, splitResultTimePoint.y, data.splits[i].getTime()).attr({ fill: "#F9F4F4", fontSize: 28, fontFamily: "Arial", fontStyle: "italic" });
                    this.align(splitTime, Alignment.leftMiddle, splitResultTimePoint);
                    // var splitTimeBox = splitTime.getBBox();
                    // splitTime.attr({ y: splitResultTimePoint.y + splitTimeBox.height / 2 });
                    var splitResultPositionPoint = data.splits[i].point.offset(this.splitResultPositionOffset);
                    var splitPosition = paper.text(splitResultPositionPoint.x, splitResultPositionPoint.y, data.splits[i].position.toString()).attr({ fill: "#676868", textAnchor: "middle", fontSize: "48px", fontFamily: "Arial", fontStyle: "bold" });
                    var splitPositionBox = splitPosition.getBBox();
                    splitPosition.attr({ y: splitResultPositionPoint.y + splitPositionBox.height / 2 });
                    var splitResultLabelPoint = data.splits[i].point.offset(this.splitResultLabelOffset);
                    var splitLabel = paper.text(splitResultLabelPoint.x, splitResultLabelPoint.y, data.splits[i].name).attr({ fill: "#676868", fontSize: "18px", fontFamily: "Arial" });
                    this.align(splitLabel, Alignment.centerMiddle, splitResultLabelPoint);
                    var splitResultGroup = paper.group(splitResultSymbol, splitTime, splitPosition, splitLabel);
                    var hide = data.splits.length > this.maxVisibleSplitResults;
                    if (hide) {
                        splitResultGroup.attr({ opacity: "0" });
                        circle.hover(this.showEventHandler, this.hideEventHandler);
                    }
                }
                if (i === data.splits.length - 1) {
                    // var resultFinish = paper.el("use", { "xlink:href": "#result_finish", x: data.splits[i].point.x, y: data.splits[i].point.y });
                    // var finishResultPoint = data.splits[i].point.offset(this.finishResultTimeOffset);
                    // var finishTime = paper.text(finishResultPoint.x, finishResultPoint.y, data.splits[i].getTime()).attr({ fill: "#676868", fontSize: "32px", fontFamily: "Arial", fontStyle: "bold" });
                    // this.align(finishTime, Alignment.centerBottom, finishResultPoint);
                    // if (this.settings.renderResultPositios) {
                    //    var finishPosition = paper.text(finishResultPoint.x, finishResultPoint.y, data.splits[i].position.toString()).attr({ fill: "#676868", fontSize: "48px", fontFamily: "Arial", fontStyle: "bold" });
                    //    this.align(finishPosition, Alignment.centerTop, finishResultPoint);
                    // }
                    // var bbox = finishPosition.getBBox();
                    // finishPosition.attr({ y: finishResultPoint.y + bbox.height });
                }
            }
        };
        //renderText(paper: Snap.Paper, text: string, position: Point, alignment: Alignment, params: Object): Snap.Element {
        //    var textElement = paper.text(position.x, position.y, text).attr(params);
        //    this.align(textElement, alignment, position);
        //    return textElement;
        //}
        ForestChart.prototype.renderLegs = function (paper, data, chartArea) {
            var _this = this;
            for (var i = 0; i < data.legs.length; i++) {
                var leg = data.legs[i];
                var legResultOffset = this.trackOffset.scaleTo(0.5);
                var legMiddlePoint = leg.middlepoint;
                var legResultPoint = legMiddlePoint.offset(legResultOffset);
                //   paper.circle(legResultPoint.x, legResultPoint.y, 3);
                var stopwatch = paper.el("use", { "xlink:href": "#stopwatch", x: legResultPoint.x, y: legResultPoint.y });
                var legResultTimePoint = legResultPoint.offset(new Vector(this.stopwatchWidth, 0));
                var legTime = Text.render(paper, leg.getTime(), legResultTimePoint, Alignment.leftBottom, { fill: "#F9F4F4", fontSize: 16, fontFamily: "Arial" });
                var legPosition = Text.render(paper, "position " + leg.position, legResultPoint, Alignment.leftTop, { fill: "#676868", fontSize: 16, fontFamily: "Arial" });
                var legResult = paper.group(stopwatch, legTime, legPosition);
                var offset = this.calcAlignmentVector(legResult, HorizontalAlignment.Center, VerticalAlignment.Middle);
                var transform = "translate(" + offset.x + "," + -offset.y / 2 + ")";
                legResult.attr({ transform: transform });
                var hide = data.splits.length > this.maxVisibleSplitResults;
                if (hide) {
                    legResult.attr({ opacity: 0 });
                    var circle = paper.circle(legMiddlePoint.x, legMiddlePoint.y, 3).attr({ fill: "#FFFFFF", stroke: "#000000", strokeWidth: 1 });
                    circle.hover(function (event) { _this.show(legResult); }, function (event) { _this.hide(legResult); });
                }
                //this.align(legResult, Alignment.centerMiddle, legResultPoint);
            }
        };
        ForestChart.prototype.render = function (profile, result, width) {
            var surfaceArea = this.surfaceArea;
            var widthMargin = surfaceArea.width / 20;
            var heightMargin = surfaceArea.height / 3;
            var headerArea = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin * 2));
            var chartArea = surfaceArea.apply(new Margin(widthMargin * 2, heightMargin, widthMargin, 40));
            var profileArea = chartArea.apply(new Margin(0, 120, 0, 120));
            var data = new ChartData(profile, result, profileArea);
            var height = (surfaceArea.height / surfaceArea.width) * width;
            var paper = Snap("#forestChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });
            this.clearChart();
            this.renderBackground(paper, surfaceArea);
            this.renderHeader(paper, data, headerArea);
            this.renderProfile(paper, data, surfaceArea, profileArea);
            this.renderSplits(paper, data, chartArea);
            //this.renderLegs(paper, data, chartArea);
            //var group = paper.g();
            //paper.circle(100, 200, 3);
            //paper.el("use", { "xlink:href": "#tree1", x: 100, y: 200 });
            //paper.circle(200, 200, 3);
            //paper.el("use", { "xlink:href": "#tree2", x: 200, y: 200 });
            //paper.circle(300, 200, 3);
            //paper.el("use", { "xlink:href": "#tree3", x: 300, y: 200, fill: "#173900" });
            //paper.circle(400, 200, 3);
            //paper.el("use", { "xlink:href": "#tree4", x: 400, y: 200, fill: "#173900" });
            //paper.circle(400, 200, 3);
            //paper.el("use", { "xlink:href": "#owl_eyes", x: 400, y: 200 });
            //paper.circle(500, 200, 3);
            //paper.el("use", { "xlink:href": "#owl", x: 500, y: 200 });
            //paper.circle(600, 200, 3);
            //paper.el("use", { "xlink:href": "#stopwatch", x: 600, y: 200 });
            //paper.circle(800, 200, 3);
            //paper.el("use", { "xlink:href": "#skier", x: 800, y: 200 });
            //paper.circle(900, 200, 3);
            //paper.el("use", { "xlink:href": "#finish_flag", x: 900, y: 200 });
            //paper.circle(1000, 200, 3);
            //paper.el("use", { "xlink:href": "#result_split", x: 1000, y: 200 });
            //paper.circle(1200, 200, 3);
            //paper.el("use", { "xlink:href": "#result_finish", x: 1200, y: 200 });
            //});
        };
        return ForestChart;
    }(Chart));
    ProfileChart.ForestChart = ForestChart;
    var ChartRenderingSettings = (function () {
        function ChartRenderingSettings(chartId, name, renderPersonProfile, renderResultPositios, renderSkierAndTrack) {
            this.chartId = chartId;
            this.name = name;
            this.renderPersonProfile = renderPersonProfile;
            this.renderResultPositios = renderResultPositios;
            this.renderSkierAndTrack = renderSkierAndTrack;
        }
        return ChartRenderingSettings;
    }());
    ProfileChart.ChartRenderingSettings = ChartRenderingSettings;
    var ChartProfile = (function () {
        function ChartProfile(trackPoints) {
            this.trackPoints = trackPoints;
            this.pipeline = new PointProcessorPipeLine();
        }
        ChartProfile.prototype.addTransform = function (transform) {
            this.pipeline.add(transform);
            this.points = this.pipeline.process(this.trackPoints);
        };
        ChartProfile.prototype.toPoint = function (trackpoint) {
            return this.pipeline.processPoint(new Point(trackpoint.distance, trackpoint.altitude));
        };
        ChartProfile.prototype.getSegmentPoints = function (start, end) {
            var segmentPoints = new Array();
            var startPoint = this.toPoint(start);
            var endPoint = this.toPoint(end);
            for (var i = 0; i < this.points.length; i++) {
                if (this.points[i].x >= startPoint.x && this.points[i].x <= endPoint.x) {
                    segmentPoints.push(this.points[i]);
                }
            }
            return segmentPoints;
        };
        return ChartProfile;
    }());
    var TrackpointConverter = (function () {
        function TrackpointConverter(pipeline) {
            this.pipeline = pipeline;
        }
        TrackpointConverter.prototype.toPoint = function (trackpoint) {
            return this.pipeline.processPoint(new Point(trackpoint.distance, trackpoint.altitude));
        };
        return TrackpointConverter;
    }());
    var ChartData = (function () {
        function ChartData(profile, result, target) {
            console.log(JSON.stringify(result));
            this.courseName = profile.name;
            this.athlete = result.athlete;
            this.track = profile.track;
            this.splits = [];
            this.legs = [];
            this.courseProfile = [];
            this.ascending = 0.0;
            this.descending = 0.0;
            for (var j = 0; j < profile.track.points.length; j++) {
                this.courseProfile.push(new Point(profile.track.points[j].distance, profile.track.points[j].altitude));
            }
            var extent = new Extent(this.courseProfile);
            this.profileExtent = extent.toRectangle();
            this.distanceAxis = new DistanceAxis(0, profile.track.length);
            this.altitudeAxis = new AltitudeAxis(extent.minY, extent.maxY);
            var pipeline = new PointProcessorPipeLine();
            var source = new Rectangle(this.distanceAxis.min, this.altitudeAxis.min, this.distanceAxis.getSpan(), this.altitudeAxis.getSpan());
            this.transform = new TransformProcessor(source, target);
            pipeline.add(this.transform);
            this.profile = new ChartProfile(this.courseProfile);
            this.profile.addTransform(this.transform);
            this.courseProfile = pipeline.process(this.courseProfile);
            var converter = new TrackpointConverter(pipeline);
            this.places = new Array();
            var splitPlaces = new Array();
            for (var i = 0; i < profile.places.length; i++) {
                if (profile.places[i].split)
                    splitPlaces.push(profile.places[i]);
                else
                    this.places.push(new ChartPlace(profile.places[i].place.name, profile.places[i].point.distance, profile.places[i].point.altitude, converter.toPoint(profile.places[i].point)));
            }
            for (var i = 0; i < splitPlaces.length; i++) {
                this.splits.push(new ChartSplit(splitPlaces[i].place.name, splitPlaces[i].point.distance, splitPlaces[i].point.altitude, converter.toPoint(splitPlaces[i].point), result.splits[i]));
            }
            for (var i = 0; i < profile.legs.length; i++) {
                var leg = profile.legs[i];
                this.legs.push(new ChartLeg(converter.toPoint(leg.startPoint), converter.toPoint(leg.middlePoint), converter.toPoint(leg.endPoint), leg.length, leg.ascending, leg.descending));
                this.ascending += leg.ascending;
                this.descending += leg.descending;
            }
            this.elapsedSeconds = this.getLastSplit().elapsedSeconds;
            this.position = this.getLastSplit().position;
        }
        ChartData.prototype.getFirstSplit = function () {
            return this.splits[0];
        };
        ChartData.prototype.getLastSplit = function () {
            return this.splits[this.splits.length - 1];
        };
        return ChartData;
    }());
    ProfileChart.ChartData = ChartData;
    var ChartPlace = (function () {
        function ChartPlace(name, distance, altitude, point) {
            this.name = name;
            this.distance = distance;
            this.altitude = altitude;
            this.point = point;
        }
        return ChartPlace;
    }());
    ProfileChart.ChartPlace = ChartPlace;
    var ChartSplit = (function (_super) {
        __extends(ChartSplit, _super);
        function ChartSplit(name, distance, altitude, point, resultSplit) {
            var _this = _super.call(this, name, distance, altitude, point) || this;
            _this.name = name;
            _this.distance = distance;
            _this.altitude = altitude;
            _this.point = point;
            _this.position = resultSplit.totalPosition;
            _this.elapsedSeconds = resultSplit.totalTimeSeconds;
            return _this;
        }
        ChartSplit.prototype.getTime = function () {
            var elapsedTime = new ElapsedTime(this.elapsedSeconds);
            return elapsedTime.toString();
        };
        return ChartSplit;
    }(ChartPlace));
    ProfileChart.ChartSplit = ChartSplit;
    var ChartLeg = (function () {
        function ChartLeg(startpoint, middlepoint, endpoint, length, ascending, descending) {
            this.startpoint = startpoint;
            this.middlepoint = middlepoint;
            this.endpoint = endpoint;
            this.length = length;
            this.ascending = ascending;
            this.descending = descending;
        }
        ChartLeg.prototype.getTime = function () {
            return new ElapsedTime(this.elapsedSeconds).toString();
        };
        return ChartLeg;
    }());
    ProfileChart.ChartLeg = ChartLeg;
    var ChartAxis = (function () {
        function ChartAxis(min, max, gridMinor, gridMajor) {
            this.min = min;
            this.max = max;
            this.gridMinor = gridMinor;
            this.gridMajor = gridMajor;
        }
        ChartAxis.calcGridBigFactor = function (gridFactor) {
            if (gridFactor <= 1) {
                return 2;
            }
            if (gridFactor <= 2) {
                return 5;
            }
            if (gridFactor <= 5) {
                return 10;
            }
            return 10;
        };
        ChartAxis.calcGridFactor = function (remainderMax) {
            if (remainderMax > 5) {
                return 5;
            }
            if (remainderMax > 2) {
                return 2;
            }
            return 1;
        };
        ChartAxis.prototype.getSpan = function () {
            return this.max - this.min;
        };
        ChartAxis.prototype.major = function (altitude) {
            return (altitude % this.gridMajor) === 0;
        };
        return ChartAxis;
    }());
    var DistanceUnit = (function () {
        function DistanceUnit(value, suffix) {
            this.value = value;
            this.suffix = suffix;
        }
        return DistanceUnit;
    }());
    DistanceUnit.meter = new DistanceUnit(1, "m");
    DistanceUnit.kilometer = new DistanceUnit(1000, "km");
    ProfileChart.DistanceUnit = DistanceUnit;
    //enum DistanceUnit {
    //    Meter = 1,
    //    Kilometer = 1000
    //}
    var DistanceAxis = (function (_super) {
        __extends(DistanceAxis, _super);
        function DistanceAxis(min, max) {
            var _this = this;
            var log10Max = Math.floor(Math.log(max - min) / Math.LN10);
            var remainderMax = Math.floor((max - min) / Math.pow(10, log10Max));
            var unit = log10Max < 3 ? DistanceUnit.meter : DistanceUnit.kilometer;
            var log10Unit = unit === DistanceUnit.kilometer ? 3 : 0;
            var log10Values = log10Max - log10Unit - 2;
            var gridFactor = ChartAxis.calcGridFactor(remainderMax);
            var gridMinor = Math.pow(10, Math.max(log10Max - 1, 0)) * gridFactor;
            var gridMajor = Math.pow(10, Math.max(log10Max - 1, 0)) * ChartAxis.calcGridBigFactor(gridFactor);
            _this = _super.call(this, min, max, gridMinor, gridMajor) || this;
            _this.unit = unit;
            _this.roundValue = Math.pow(10, log10Values);
            _this.decimals = log10Values < 0 ? Math.abs(log10Values) : 0;
            return _this;
        }
        DistanceAxis.prototype.format = function (distance, withSuffix) {
            distance = distance / this.unit.value;
            distance = Math.floor(distance / this.roundValue) * this.roundValue;
            var res = parseFloat(distance.toFixed(this.decimals)).toString();
            //for(var i = res.length - 1; i >= 0; i--)
            // if(res[i] == "0")
            if (withSuffix)
                res += " " + this.unit.suffix;
            return res;
        };
        return DistanceAxis;
    }(ChartAxis));
    ProfileChart.DistanceAxis = DistanceAxis;
    var AltitudeAxis = (function (_super) {
        __extends(AltitudeAxis, _super);
        function AltitudeAxis(minAltitude, maxAltitude) {
            var _this = this;
            var padding = 5000 / (maxAltitude - minAltitude);
            var minBase = Math.min(minAltitude, 0);
            minAltitude = Math.max(minAltitude - 3 * padding, minBase);
            maxAltitude = maxAltitude + padding;
            var altitudeSpan = maxAltitude - minAltitude;
            var log10Max = Math.floor(Math.log(altitudeSpan) / Math.LN10);
            var remainderMax = Math.floor(altitudeSpan / Math.pow(10, log10Max));
            var log10Unit = 0;
            var log10Values = log10Max - log10Unit - 2;
            var log10Grid = Math.max(log10Max - 1, 0);
            var gridFactor = ChartAxis.calcGridFactor(remainderMax);
            var gridMinor = Math.pow(10, log10Grid) * gridFactor;
            var gridMajor = Math.pow(10, log10Grid) * ChartAxis.calcGridBigFactor(gridFactor);
            var min = AltitudeAxis.calcMin(minAltitude, maxAltitude, gridMinor, gridMajor);
            var max = AltitudeAxis.calcMax(minAltitude, maxAltitude, gridMinor, gridMajor);
            _this = _super.call(this, min, max, gridMinor, gridMajor) || this;
            _this.unit = 1;
            _this.roundValue = Math.pow(10, log10Values);
            _this.decimals = 0;
            return _this;
        }
        AltitudeAxis.prototype.format = function (altitude, withSuffix) {
            altitude = Math.floor(altitude / this.roundValue) * this.roundValue;
            var res = parseFloat(altitude.toFixed(this.decimals)).toString();
            if (withSuffix)
                res += " m";
            return res;
        };
        AltitudeAxis.prototype.getRoundFactor = function (altitudeSpan, gridMinor, gridMajor) {
            var roundFactor = 5;
            if (altitudeSpan > 20)
                roundFactor = gridMajor * 5;
            if (altitudeSpan > 50)
                roundFactor = gridMajor * 2;
            if (altitudeSpan > 100)
                roundFactor = gridMajor;
            if (altitudeSpan > 500)
                roundFactor = gridMinor;
            return roundFactor;
        };
        AltitudeAxis.prototype.getPadding = function (altitudeSpan) {
            return 5000 / altitudeSpan;
            //var padding: number = 0;
            //if (altitudeSpan < 200)
            //    padding = 10;
            //if (altitudeSpan < 100)
            //    padding = 50;
            //if (altitudeSpan < 20)
            //    padding = 100;
            //if (altitudeSpan < 10)
            //    padding = 200;
            //return padding;
        };
        AltitudeAxis.calcMin = function (minAltitude, maxAltitude, gridMinor, gridMajor) {
            var roundFactor = gridMajor;
            var minBase = Math.min(minAltitude, 0);
            return Math.max(Math.floor(minAltitude / roundFactor) * roundFactor, minBase);
        };
        AltitudeAxis.calcMax = function (minAltitude, maxAltitude, gridMinor, gridMajor) {
            var roundFactor = gridMajor;
            return Math.ceil(maxAltitude / roundFactor) * roundFactor;
        };
        return AltitudeAxis;
    }(ChartAxis));
    ProfileChart.AltitudeAxis = AltitudeAxis;
    var PointProcessor = (function () {
        function PointProcessor(name) {
            this.name = name;
        }
        PointProcessor.prototype.process = function (points) {
            return null;
        };
        PointProcessor.prototype.processPoint = function (point) {
            return null;
        };
        return PointProcessor;
    }());
    var PointProcessorPipeLine = (function (_super) {
        __extends(PointProcessorPipeLine, _super);
        function PointProcessorPipeLine() {
            var _this = _super.call(this, "pipeline") || this;
            _this.processors = new Array();
            return _this;
        }
        PointProcessorPipeLine.prototype.add = function (processor) {
            this.processors.push(processor);
            return this;
        };
        PointProcessorPipeLine.prototype.processPoint = function (point) {
            return this.process([point])[0];
        };
        PointProcessorPipeLine.prototype.process = function (points) {
            for (var i = 0; i < this.processors.length; i++)
                points = this.processors[i].process(points);
            return points;
        };
        return PointProcessorPipeLine;
    }(PointProcessor));
    var TransformProcessor = (function (_super) {
        __extends(TransformProcessor, _super);
        function TransformProcessor(source, target) {
            var _this = _super.call(this, "Transform") || this;
            _this.source = source;
            _this.target = target;
            return _this;
        }
        TransformProcessor.prototype.processCoordinate = function (x, y) {
            return this.processPoint(new Point(x, y));
        };
        TransformProcessor.prototype.processPoint = function (point) {
            return this.process([point])[0];
        };
        TransformProcessor.prototype.process = function (points) {
            var result = [];
            var scaleX = this.target.width / this.source.width;
            var scaleY = this.source.height > 0 ? this.target.height / this.source.height : 1;
            for (var i = 0; i < points.length; i++) {
                var x = this.target.x + (points[i].x - this.source.x) * scaleX;
                var y = this.target.y + this.target.height - (points[i].y - this.source.y) * scaleY;
                result.push(new Point(x, y));
            }
            return result;
        };
        return TransformProcessor;
    }(PointProcessor));
    var ReduceProcessor = (function (_super) {
        __extends(ReduceProcessor, _super);
        function ReduceProcessor(maxlevels, epsilon) {
            var _this = _super.call(this, "Reduce") || this;
            _this.maxlevels = maxlevels;
            _this.epsilon = epsilon;
            return _this;
        }
        ReduceProcessor.prototype.process = function (points) {
            return this.douglasPeucker(points, 0);
        };
        ReduceProcessor.prototype.calcPerpendicularDistance = function (location, start, end) {
            var a = location.x - start.x;
            var b = location.y - start.y;
            var c = end.x - start.x;
            var d = end.y - start.y;
            return Math.abs(a * d - c * b) / Math.sqrt(c * c + d * d);
        };
        ReduceProcessor.prototype.calcHeightDistance = function (point, start, end) {
            var k = (end.y - start.y) / (end.x - start.x);
            var y = start.y + k * (point.x - start.x);
            return Math.abs(point.y - y);
        };
        ReduceProcessor.prototype.douglasPeucker = function (points, level) {
            var maxDistance = 0.0;
            var maxAltitudeIndex = 0;
            for (var i = 1; i < points.length - 2; i++) {
                //var distance: number = this.calcPerpendicularDistance(points[i], points[0], points[points.length - 1]);
                var distance = this.calcHeightDistance(points[i], points[0], points[points.length - 1]);
                if (distance > maxDistance) {
                    maxDistance = distance;
                    maxAltitudeIndex = i;
                }
            }
            var result = [];
            if (level <= this.maxlevels && maxDistance > this.epsilon) {
                var result1 = this.douglasPeucker(points.slice(0, maxAltitudeIndex + 1), ++level);
                var result2 = this.douglasPeucker(points.slice(maxAltitudeIndex, points.length), ++level);
                result.push.apply(result, result1.slice(0, result1.length - 1));
                result.push.apply(result, result2);
            }
            else {
                result.push(points[0]);
                result.push(points[points.length - 1]);
            }
            return result;
        };
        return ReduceProcessor;
    }(PointProcessor));
    ReduceProcessor.maxLevels = 1000;
    ReduceProcessor.minEpsilon = 0.00001;
    var ReduceToNumberProcessor = (function (_super) {
        __extends(ReduceToNumberProcessor, _super);
        function ReduceToNumberProcessor(nrOfPoints) {
            var _this = _super.call(this, "ReducePointsSegment") || this;
            _this.nrOfPoints = nrOfPoints;
            return _this;
        }
        ReduceToNumberProcessor.prototype.process = function (points) {
            if (points.length <= this.nrOfPoints)
                return points;
            var rootSegment = new ReduceSegment(points);
            for (var i = 0; i < this.nrOfPoints; i++) {
                var segment = this.findSegmentWithMaxDistance(rootSegment);
                segment.split();
            }
            var result = [];
            result.push(rootSegment.points[0]);
            result.push.apply(result, this.getPoints(rootSegment));
            result.push(rootSegment.points[rootSegment.points.length - 1]);
            return result;
        };
        ReduceToNumberProcessor.prototype.getPoints = function (segment) {
            var points = new Array();
            if (!segment.splitted)
                return points;
            points.push.apply(points, this.getPoints(segment.firstSubSegment));
            points.push(segment.maxDistancePoint);
            points.push.apply(points, this.getPoints(segment.secondSubSegment));
            return points;
        };
        ReduceToNumberProcessor.prototype.findSegmentWithMaxDistance = function (segment) {
            if (!segment.splitted)
                return segment;
            var firstSegment = this.findSegmentWithMaxDistance(segment.firstSubSegment);
            var secondSegment = this.findSegmentWithMaxDistance(segment.secondSubSegment);
            if (firstSegment.maxDistance >= secondSegment.maxDistance)
                return firstSegment;
            return secondSegment;
        };
        return ReduceToNumberProcessor;
    }(PointProcessor));
    var ReduceSegment = (function () {
        function ReduceSegment(points) {
            this.points = points;
            this.maxDistance = 0.0;
            this.maxDistanceIndex = 0.0;
            this.maxDistancePoint = null;
            this.splitted = false;
            this.firstSubSegment = null;
            this.secondSubSegment = null;
            for (var i = 1; i < points.length - 1; i++) {
                var distance = points[i].heightDistanceTo(points[0], points[points.length - 1]);
                if (distance > this.maxDistance) {
                    this.maxDistance = distance;
                    this.maxDistanceIndex = i;
                    this.maxDistancePoint = points[i];
                }
            }
            if (this.maxDistancePoint == null) {
                this.maxDistanceIndex = Math.floor(1 + points.length / 2);
                this.maxDistancePoint = points[this.maxDistanceIndex];
            }
        }
        ReduceSegment.prototype.split = function () {
            this.firstSubSegment = new ReduceSegment(this.points.slice(0, this.maxDistanceIndex + 1));
            this.secondSubSegment = new ReduceSegment(this.points.slice(this.maxDistanceIndex, this.points.length));
            this.splitted = true;
        };
        return ReduceSegment;
    }());
    var OffsetProcessor = (function (_super) {
        __extends(OffsetProcessor, _super);
        function OffsetProcessor(offset) {
            var _this = _super.call(this, "Offset") || this;
            _this.offset = offset;
            return _this;
        }
        OffsetProcessor.prototype.process = function (points) {
            var offsetedPoints = [];
            for (var i = 0; i < points.length; i++) {
                offsetedPoints.push(points[i].offset(this.offset));
            }
            return offsetedPoints;
        };
        OffsetProcessor.prototype.processPoint = function (point) {
            return point.offset(this.offset);
        };
        return OffsetProcessor;
    }(PointProcessor));
    var SkewProcessor = (function (_super) {
        __extends(SkewProcessor, _super);
        function SkewProcessor(vector) {
            var _this = _super.call(this, "Skew") || this;
            _this.vector = vector;
            return _this;
        }
        SkewProcessor.prototype.process = function (points) {
            var skewPoints = [];
            for (var i = 0; i < points.length; i++) {
                skewPoints.push(this.processPoint(points[i]));
            }
            return skewPoints;
        };
        SkewProcessor.prototype.processPoint = function (point) {
            return new Point(point.x, point.y + this.vector.scaleToX(point.x).y);
        };
        return SkewProcessor;
    }(PointProcessor));
    var MountainGenerator = (function () {
        function MountainGenerator(rectangle, levels, numberOfPoints) {
            this.rectangle = rectangle;
            this.points = new Array();
            var start = new Point(rectangle.x, rectangle.y + rectangle.height * Math.random());
            var end = new Point(rectangle.x + rectangle.width, rectangle.y + rectangle.height * Math.random());
            this.generatePoints(numberOfPoints, start, end, rectangle.height / 2, levels);
        }
        MountainGenerator.prototype.generatePoints = function (numberOfPoints, start, end, spanY, level) {
            var vector = Vector.create(start, end);
            var totalWidth = end.x - start.x;
            var segmentWidth = totalWidth / numberOfPoints;
            this.points.push(start);
            var lastPoint = start;
            for (var i = 0; i < numberOfPoints; i++) {
                var dx = i * segmentWidth + segmentWidth * Math.random();
                var dy = vector.scaleTo(dx / totalWidth).y;
                var point = new Point(start.x + dx, start.y + dy + spanY * Math.random() - spanY / 2);
                if (level > 0)
                    this.generatePoints(numberOfPoints, lastPoint, point, spanY / 6, level - 1);
                this.points.push(point);
                lastPoint = point;
            }
            if (level > 0)
                this.generatePoints(numberOfPoints, lastPoint, end, spanY / 6, level - 1);
            this.points.push(end);
        };
        return MountainGenerator;
    }());
    ProfileChart.MountainGenerator = MountainGenerator;
})(ProfileChart || (ProfileChart = {}));
//# sourceMappingURL=profile-chart.js.map