/// <reference path="../definitions/snap.svg.d.ts" />

module ProfileChart {
    "use strict";
    export class Point {
        offset(vector: Vector): Point {
            return new Point(this.x + vector.x, this.y + vector.y);
        }

        perpendicularDistanceTo(start: Point, end: Point): number {
            var a: number = this.x - start.x;
            var b: number = this.y - start.y;
            var c: number = end.x - start.x;
            var d: number = end.y - start.y;

            return Math.abs(a * d - c * b) / Math.sqrt(c * c + d * d);
        }

        heightDistanceTo(start: Point, end: Point): number {
            var k: number = (end.y - start.y) / (end.x - start.x);

            var y: number = start.y + k * (this.x - start.x);

            return Math.abs(this.y - y);
        }

        constructor(public x: number, public y: number) {
        }
    }

    export class Vector {

        static combineNormalized(first: Vector, second: Vector): Vector {

            var vector: Vector = new Vector(first.x + second.x, first.y + second.y);

            var dotProduct: number = first.x * second.x + first.y * second.y;

            var scale: number = 1 / (dotProduct + 1);

            return new Vector(vector.x * scale, vector.y * scale);
        }

        normalize(): Vector {
            var length: number = Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));

            return new Vector(this.x / length, this.y / length);
        }



        scaleTo(scale: number): Vector {
            return new Vector(this.x * scale, this.y * scale);
        }

        scaleToX(x: number): Vector {
            var k: number = this.y / this.x;

            return new Vector(x, x * k);
        }

        scale(scaleX: number, scaleY: number): Vector {
            return new Vector(this.x * scaleX, this.y * scaleY);
        }

        add(vector: Vector): Vector {
            return new Vector(this.x + vector.x, this.y + vector.y);
        }

        reverse(): Vector {
            return new Vector(-this.x, -this.y);
        }

        getLength(): number {
            return Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));
        }
        // static CalcCrossProduct(first: Vector, second: Vector): Vector {
        //     return new Vector(first.x * second.y) - (first.y * second.y);
        // }

        static create(from: Point, to: Point): Vector {
            return new Vector(to.x - from.x, to.y - from.y);
        }

        static createNormalized(from: Point, to: Point): Vector {
            return new Vector(to.x - from.x, to.y - from.y).normalize();
        }

        static createNormal(from: Point, to: Point): Vector {
            var dx: number = to.x - from.x;
            var dy: number = to.y - from.y;

            return new Vector(dy, -dx);
        }



        constructor(public x: number, public y: number) {
        }
    }

    class Offset {
        constructor(public width: number, public height: number) {
        }
    }

    class Size {
        width: number;
        height: number;
    }

    export class Margin {
        constructor(public left: number, public top: number, public right: number, public bottom: number) {
        }
    }

    export class Rectangle {
        getCenter(): Point {
            return new Point(this.x + this.width / 2, this.y + this.height / 2);
        }

        apply(margin: Margin): Rectangle {
            return new Rectangle(this.x + margin.left,
                this.y + margin.top,
                this.width - margin.left - margin.right,
                this.height - margin.top - margin.bottom);
        }

        constructor(public x: number, public y: number, public width: number, public height: number) {
        }
    }

    class Extent {
        minX: number = Number.MAX_VALUE;
        maxX: number = -Number.MAX_VALUE;
        minY: number = Number.MAX_VALUE;
        maxY: number = -Number.MAX_VALUE;


        containPoint(point: Point): void {
            this.minX = Math.min(this.minX, point.x);
            this.maxX = Math.max(this.maxX, point.x);
            this.minY = Math.min(this.minY, point.y);
            this.maxY = Math.max(this.maxY, point.y);
        }

        containPoints(points: Point[]): void {
            for (var i: number = 0; i < points.length; i++) {
                this.containPoint(points[i]);
            }
        }

        getWidth(): number {
            return this.maxX - this.minX;
        }

        getHeight(): number {
            return this.maxY - this.minY;
        }

        getRatio(): number {
            return this.getWidth() / this.getHeight();
        }

        toRectangle(): Rectangle {
            return new Rectangle(this.minX, this.minY, this.maxX - this.minX, this.maxY - this.minY);
        }

        constructor(points: Point[]) {
            this.containPoints(points);
        }
    }

    class ElapsedTime {
        pad(num: number, size: number): string {
            var s: string = num + "";

            while (s.length < size) {
                s = "0" + s;
            }

            return s;
        }

        padZeros(num: number): string {
            return this.pad(num, 2);
        }

        getSeparator(elapsedTime: string): string {
            return elapsedTime.length > 0 ? ":" : "";
        }

        formatTime(elapsedTime: string, time: number, includeZero: Boolean): string {
            var timeString: string = "";

            if (includeZero || time > 0) {
                timeString += this.getSeparator(elapsedTime);
                timeString += this.padZeros(time);
            }

            return timeString;
        }

        toString(): string {
            var seconds: number = this.seconds;

            var days: number = Math.floor(seconds / (60 * 60 * 24));
            seconds -= days * (60 * 60 * 24);

            var hours: number = Math.floor(seconds / (60 * 60));
            seconds -= hours * (60 * 60);

            var minutes: number = Math.floor(seconds / (60));
            seconds -= minutes * (60);

            hours = days * 24 + hours;

            var elapsedTime: string = "";

            // elapsedTime += this.formatTime(elapsedTime, days, false);
            elapsedTime += this.formatTime(elapsedTime, hours, false);
            elapsedTime += this.formatTime(elapsedTime, minutes, true);
            elapsedTime += this.formatTime(elapsedTime, seconds, true);

            return elapsedTime;
        }


        constructor(private seconds: number) {
        }
    }

    class ChartType {
        splitSymbolTimeOffset: Offset;
        splitSymbolPositionOffset: Offset;

        // transformData(data: ChartData, chartArea: Rectangle)
        // {
        //    var transform = new TransformProcessor(chartArea);

        //    data.courseProfile = transform.Process(data.courseProfile);

        //    return data;
        // }

        constructor(public name: string, public templateUrl: string) {
        }
    }

    class ControlPointValues {
        constructor(public p1: Array<number>, public p2: Array<number>) {
        }
    }

    export class Chart {
        public surfaceArea: Rectangle = new Rectangle(0, 0, 1900, 1000);

        clearChart(): void {
            Snap.selectAll("SVG > *:not(defs)").remove();
        }

        getTemplateUrl(): string {
            return this.templateUrl;
        }

        createCurveToPath(points: Point[]): string {
            var path: string = "M" + points[0].x + " " + points[0].y;

            var operation: string = "C";

            for (var i: number = 1; i < points.length - 1; i++) {
                path += operation + points[i - 1].x + "," + points[i - 1].y + " " +
                    points[i + 1].x + "," + points[i + 1].y + " " +
                    points[i].x + "," + points[i].y;
            }

            return path;
        }

        toPathString(points: Point[]): string {
            var path: string = "";

            var operation: string = "M";

            for (var i: number = 0; i < points.length; i++) {
                path += operation + points[i].x + " " + points[i].y;

                if (i === 0) {
                    operation = "L";
                }
            }

            return path;
        }

        createCurveToPathT(points: Point[]): string {
            var path: string = "";

            var operation: string = "M";

            for (var i: number = 0; i < points.length; i++) {
                path += operation + points[i].x + " " + points[i].y;

                if (i === 0) {
                    operation = "T";
                }
            }

            return path;
        }

        createCurveThroughPath(points: Point[]): string {
            var path: string = "";

            var x: Array<number> = new Array();
            var y: Array<number> = new Array();

            for (var i: number = 0; i < points.length; i++) {
                x[i] = points[i].x;
                y[i] = points[i].y;
            }

            /*computes control points p1 and p2 for x and y direction*/
            var px: ControlPointValues = this.computeControlPoints(x);
            var py: ControlPointValues = this.computeControlPoints(y);

            for (var j: number = 1; j < points.length; j++) {
                var controlPoint1: Point = new Point(px.p1[j - 1], py.p1[j - 1]);
                var controlPoint2: Point = new Point(px.p2[j - 1], py.p2[j - 1]);

                path += this.curvePath(points[j - 1], controlPoint1, controlPoint2, points[j]);
            }

            return path;
        }

        /*creates formated path string for SVG cubic path element*/
        curvePath(from: Point, controlPoint1: Point, controlPoint2: Point, to: Point): string {
            return "M " + from.x + "," + from.y + " C " + controlPoint1.x + "," + controlPoint1.y + " " + controlPoint2.x + "," + controlPoint2.y + " " + to.x + "," + to.y;
        }

        /*computes control points given knots K, this is the brain of the operation*/
        computeControlPoints(k: Array<number>): ControlPointValues {
            var p1: Array<number> = new Array();
            var p2: Array<number> = new Array();
            var n: number = k.length - 1;

            /*rhs vector*/
            var a: Array<number> = new Array();
            var b: Array<number> = new Array();
            var c: Array<number> = new Array();
            var r: Array<number> = new Array();

            /*left most segment*/
            a[0] = 0;
            b[0] = 2;
            c[0] = 1;
            r[0] = k[0] + 2 * k[1];

            /*internal segments*/
            for (let i: number = 1; i < n - 1; i++) {
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
            for (let i: number = 1; i < n; i++) {
                var m: number = a[i] / b[i - 1];
                b[i] = b[i] - m * c[i - 1];
                r[i] = r[i] - m * r[i - 1];
            }

            p1[n - 1] = r[n - 1] / b[n - 1];
            for (let i: number = n - 2; i >= 0; --i) {
                p1[i] = (r[i] - c[i] * p1[i + 1]) / b[i];
            }

            /*we have p1, now compute p2*/
            for (let i: number = 0; i < n - 1; i++) {
                p2[i] = 2 * k[i + 1] - p1[i + 1];
            }

            p2[n - 1] = 0.5 * (k[n] + p1[n - 1]);

            // return { p1: p1, p2: p2 };
            return new ControlPointValues(p1, p2);
        }

        calcAlignmentVector(el: Snap.Element, horizontal: HorizontalAlignment, vertical: VerticalAlignment): Vector {
            var bbox: Snap.BBox = el.getBBox();

            var x: number = 0.0;
            var y: number = 0.0;

            if (horizontal === HorizontalAlignment.Center) {
                x = bbox.width / -2;
            } else if (horizontal === HorizontalAlignment.Right) {
                x = -bbox.width;
            }

            if (vertical === VerticalAlignment.Middle) {
                y = bbox.height / 2;
            } else if (vertical === VerticalAlignment.Top) {
                y = bbox.height;
            }

            return new Vector(x, y);
        }

        align(el: Snap.Element, alignment: Alignment, position: Point): void {
            var pos: Point = position.offset(this.calcAlignmentVector(el, alignment.horizontal, alignment.vertical));

            el.attr({ x: pos.x, y: pos.y });
        }

        constructor(public id: string, public name: string, public templateUrl: string) {
        }
    }

    enum HorizontalAlignment {
        Left = 0,
        Center = 1,
        Right = 2
    }

    enum VerticalAlignment {
        Top = 0,
        Middle = 1,
        Bottom = 2
    }

    export class Alignment {
        static leftTop: Alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);
        static leftMiddle: Alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Middle);
        static leftBottom: Alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);

        static centerTop: Alignment = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Top);
        static centerMiddle: Alignment = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Middle);
        static centerBottom: Alignment = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);

        static rightTop: Alignment = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Top);
        static rightMiddle: Alignment = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Middle);
        static rightBottom: Alignment = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Bottom);

        constructor(public horizontal: HorizontalAlignment, public vertical: VerticalAlignment) {
        }
    }

    class Text {
        public static calcAlignmentVector(el: Snap.Element, horizontal: HorizontalAlignment, vertical: VerticalAlignment): Vector {
            var bbox: Snap.BBox = el.getBBox();

            var x: number = 0.0;
            var y: number = 0.0;

            if (horizontal === HorizontalAlignment.Center) {
                x = bbox.width / -2;
            } else if (horizontal === HorizontalAlignment.Right) {
                x = -bbox.width;
            }

            if (vertical === VerticalAlignment.Middle) {
                y = bbox.height / 2;
            } else if (vertical === VerticalAlignment.Top) {
                y = bbox.height;
            }

            return new Vector(x, y);
        }

        public static align(el: Snap.Element, alignment: Alignment, position: Point): void {
            var pos: Point = position.offset(this.calcAlignmentVector(el, alignment.horizontal, alignment.vertical));

            el.attr({ x: pos.x, y: pos.y });
        }

        public static render(paper: Snap.Paper, text: string, position: Point, alignment: Alignment, params: any): Snap.Element {
            var textElement: Snap.Element = paper.text(position.x, position.y, text);
            textElement.attr(params);

            this.align(textElement, alignment, position);

            return textElement;
        }
    }

    export class LoadingChart extends Chart {

        renderBackground(paper: Snap.Paper, surfaceArea: Rectangle): void {
            var g: Snap.Paper = paper.gradient("l(0, 1, 1, 0)#bebebe-#fefefe");

            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        }

        render(profile: IProfile, result: IResult, width: number): void {
            var surfaceArea: Rectangle = this.surfaceArea;

            var height: number = (surfaceArea.height / surfaceArea.width) * width;

            var paper: Snap.Paper = Snap("#loadingChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });

            this.clearChart();

            this.renderBackground(paper, surfaceArea);

            Text.render(paper, "Loading...", surfaceArea.getCenter(), Alignment.centerMiddle, { fontSize: "72px" });
        }

        public static id: string = "73CE29D6-AE8F-405C-BA21-C267F81AEFC5";

        constructor() {
            super(LoadingChart.id, "Loading", "/App/ProfileChart/Templates/loading.svg");
        }
    }

    export class TestChart extends Chart {

        renderBackground(paper: Snap.Paper, surfaceArea: Rectangle): void {
            var g: any = paper.gradient("l(0, 1, 1, 0)#bebebe-#fefefe");

            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        }

        renderHeader(paper: Snap.Paper, data: ChartData, area: Rectangle): void {
            var textPoint: Point = new Point(area.x + area.width / 2, area.y);

            var courseNameText: Snap.Element = paper.text(textPoint.x, textPoint.y, data.courseName);
            courseNameText.attr({ textAnchor: "middle", fontSize: "20px", fill: "#2ba7de" });

            var bbox: Snap.BBox = courseNameText.getBBox();
            courseNameText.attr({ y: textPoint.y + bbox.height });
        }

        renderProfile(paper: Snap.Paper, data: ChartData, profileArea: Rectangle): void {

            var extent: Extent = new Extent([]);
            extent.containPoints(data.courseProfile);

            var pipeline: PointProcessorPipeLine = new PointProcessorPipeLine();

            var reduce: PointProcessor = new ReduceToNumberProcessor(50);

            pipeline.add(reduce);

            var profile: Point[] = pipeline.process(data.courseProfile);

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

            var color: string = "#666666";

            paper.path(super.toPathString(profile)).attr({ fill: "none", stroke: color, strokeWidth: 1 });

            for (var i: number = 0; i < profile.length; i++) {
                var dot: Snap.Element = paper.circle(profile[i].x, profile[i].y, 3);
                dot.attr({ fill: "#333333" });

                var prev: Point = profile[i - 1];
                var point: Point = profile[i];
                var next: Point = profile[i + 1];

                var textOffset: Vector = this.calcTextOffset(prev, point, next, 15);

                var textPoint: Point = profile[i].offset(textOffset);

                var text: Snap.Element = paper.text(textPoint.x, textPoint.y, i.toString());
                text.attr({ textAnchor: "middle", dominantBaseline: "middle", fill: "#333333", fontSize: 13 });
            }
        }

        calcTextOffset(prev: Point, point: Point, next: Point, distance: number): Vector {
            var direction: Vector;

            if (prev === undefined) {
                direction = Vector.createNormal(point, next);
            } else if (next === undefined) {
                direction = Vector.createNormal(prev, point);
            } else {
                var firstVector: Vector = Vector.createNormalized(prev, point);
                var secondVector: Vector = Vector.createNormalized(next, point);

                direction = firstVector.add(secondVector);
            }

            var vector: Vector = direction.normalize().scaleTo(distance);

            return vector;
        }

        render(profile: IProfile, result: IResult, width: number): void {
            var surfaceArea: Rectangle = this.surfaceArea;

            var widthMargin: number = surfaceArea.width / 20;
            var heightMargin: number = surfaceArea.height / 3;

            var headerArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin * 2));
            var profileArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, heightMargin, widthMargin, heightMargin));

            var data: ChartData = new ChartData(profile, result, profileArea);

            var height: number = (surfaceArea.height / surfaceArea.width) * width;

            var paper: Snap.Paper = Snap("#testChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });

            this.clearChart();

            this.renderBackground(paper, surfaceArea);
            this.renderHeader(paper, data, headerArea);

            this.renderProfile(paper, data, profileArea);
        }

        public static id: string = "4F0B4EC0-6E72-44FD-9F26-F4423D7CE973";

        constructor() {
            super(TestChart.id, "Connecting dots", "/App/ProfileChart/Templates/sdftest.svg");
        }
    }


    export class MountainSilhouetteChart extends Chart {

        renderBackground(paper: Snap.Paper, surfaceArea: Rectangle): void {
            var g: any = paper.gradient("l(0, 1, 1, 0)#DDD6C6-#fefefe");

            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        }

        renderMountain(paper: Snap.Paper, topArea: Rectangle, baseY: number, color: string): void {

            var points: Point[] = new MountainGenerator(topArea, 2, 5).points;

            var profileBody: Point[] = [];

            profileBody.push(new Point(topArea.x, topArea.y));

            profileBody.push.apply(profileBody, points);

            profileBody.push(new Point(topArea.x + topArea.width, points[points.length - 1].y));
            profileBody.push(new Point(topArea.x + topArea.width, baseY));
            profileBody.push(new Point(topArea.x, baseY));
            profileBody.push(new Point(topArea.x, points[0].y));

            var bodyPathString: string = super.toPathString(profileBody) + " Z";

            paper.path(bodyPathString).attr({ fill: color });
        }

        renderProfile(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {
            var reduce: PointProcessor = new ReduceToNumberProcessor(100);

            var points: Point[] = reduce.process(data.courseProfile);

            var profileBody: Point[] = [];

            profileBody.push(new Point(chartArea.x, chartArea.y));

            profileBody.push.apply(profileBody, points);

            profileBody.push(new Point(chartArea.x + chartArea.width, points[points.length - 1].y));
            profileBody.push(new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, points[0].y));

            var bodyPathString: string = super.toPathString(profileBody) + " Z";

            paper.path(bodyPathString).attr({ fill: "#0A0D14" });
        }

        render(profile: IProfile, result: IResult, width: number): void {
            var surfaceArea: Rectangle = this.surfaceArea;

            // var widthMargin: number = surfaceArea.width / 20;
            var heightMargin: number = surfaceArea.height / 2;

            var mountainArea: Rectangle = surfaceArea.apply(new Margin(10, heightMargin / 2, 10, 10));

            // var headerArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin));
            var chartArea: Rectangle = surfaceArea.apply(new Margin(10, heightMargin, 10, 10));

            var data: ChartData = new ChartData(profile, result, chartArea);

            var profileExtent: Extent = new Extent(data.courseProfile);

            var height: number = (surfaceArea.height / surfaceArea.width) * width;

            var paper: Snap.Paper = Snap("#mountainSilhouetteChart");
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

            var frontProfileHeight: number = chartArea.height - (profileExtent.maxY - chartArea.y);
            var frontProfileTop: number = profileExtent.maxY;

            this.renderMountain(paper, new Rectangle(mountainArea.x, frontProfileTop, mountainArea.width, frontProfileHeight), mountainArea.y + mountainArea.height, "#333333");
            this.renderMountain(paper, new Rectangle(mountainArea.x, frontProfileTop, mountainArea.width, frontProfileHeight), mountainArea.y + mountainArea.height, "#222222");
        }

        public static id: string = "28D33FB8-BEFC-41B3-B947-A0B9B6A811EB";

        constructor() {
            super(MountainSilhouetteChart.id, "Mountain Silhouette", "/App/ProfileChart/Templates/mountain_silhouette.svg");
        }
    }


    export class ConnectingDotsChart extends Chart {

        renderPaperCorner(paper: Snap.Paper, location: Point, direction: Vector): void {
            var points: Array<Point> = new Array<Point>();

            points.push(location);
            points.push(new Point(location.x + direction.x, location.y));
            points.push(new Point(location.x + direction.x/100, location.y + direction.y/100));
            points.push(new Point(location.x, location.y + direction.y));
            points.push(location);

            paper.path(super.toPathString(points) + " Z").attr({ fill: "#666666", stroke: "none"}); // filter: "url(#penFilter)"
        }

        renderBackgroundGrid(paper: Snap.Paper, paperArea: Rectangle): void {
            var gridSize: number = 32;

            // paper.rect(paperArea.x, paperArea.y, paperArea.width, paperArea.height).attr({ stroke: "#A4A3A3", fill: "none" });

            for (var x: number = paperArea.x + gridSize; x <= paperArea.x + paperArea.width - gridSize; x += gridSize) {
                paper.line(x, paperArea.y, x, paperArea.y + paperArea.height).attr({ stroke: "#cccccc", strokeWidth: 0.5, fill: "#cccccc" });
            }

            for (var y: number = paperArea.y + gridSize; y <= paperArea.y + paperArea.height - gridSize; y += gridSize) {
                paper.line(paperArea.x, y, paperArea.x + paperArea.width, y).attr({ stroke: "#cccccc", strokeWidth: 0.5, fill: "#cccccc" });
            }
        }

        renderBackground(paper: Snap.Paper, paperArea: Rectangle): void {
            this.renderBackgroundGrid(paper, paperArea);

            var size: number = paperArea.width / 8;

            this.renderPaperCorner(paper, new Point(paperArea.x, paperArea.y), new Vector(size, size));
            this.renderPaperCorner(paper, new Point(paperArea.x + paperArea.width, paperArea.y), new Vector(-size, size));
            this.renderPaperCorner(paper, new Point(paperArea.x + paperArea.width, paperArea.y + paperArea.height), new Vector(-size, -size));
            this.renderPaperCorner(paper, new Point(paperArea.x, paperArea.y + paperArea.height), new Vector(size, -size));
        }

        renderHeader(paper: Snap.Paper, data: ChartData, headerArea: Rectangle): void {

            var topLeftPoint: Point = new Point(headerArea.x, headerArea.y);
            var topRightPoint: Point = new Point(headerArea.x + headerArea.width, headerArea.y);

            Text.render(paper, data.splits[0].name, topLeftPoint.offset(new Vector(20, 20)), Alignment.leftTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.splits[0].altitude.toFixed(0) + " m", topLeftPoint.offset(new Vector(20, 60)), Alignment.leftTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });

            Text.render(paper, data.getLastSplit().name, topRightPoint.offset(new Vector(-20, 20)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " m", topRightPoint.offset(new Vector(-20, 60)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });


            var athleteDisplayNamePoint: Point = headerArea.getCenter();

            Text.render(paper, data.athlete.displayName, athleteDisplayNamePoint.offset(new Vector(0, -20)), Alignment.centerBottom, { fill: "#1d5f8d", fontSize: "64px" });

            var courseNamePoint: Point = headerArea.getCenter();

            Text.render(paper, data.courseName, courseNamePoint, Alignment.centerTop, { fill: "#1d5f8d", fontSize: "72px" });
        }

        renderProfile(paper: Snap.Paper, data: ChartData, profileArea: Rectangle): void {
            // paper.rect(profileArea.x, profileArea.y, profileArea.width, profileArea.height).attr({ fill:"none", stroke: "#FF0000" });

            var pipeline: PointProcessorPipeLine = new PointProcessorPipeLine();

            var reduce: PointProcessor = new ReduceToNumberProcessor(36);

            pipeline.add(reduce);

            var profile: Point[] = pipeline.process(data.courseProfile);

            if (profile.length > 0) {
                paper.el("use", { "xlink:href": "#startHere", x: profile[0].x, y: profile[0].y });
                paper.el("use", { "xlink:href": "#finishPen", x: profile[profile.length - 1].x, y: profile[profile.length - 1].y });

                var totalResultTimePoint: Point = profile[profile.length - 1].offset(new Vector(-130, -200));

                Text.render(paper, data.getLastSplit().getTime(), totalResultTimePoint, Alignment.rightMiddle, { fill: "#000000", fontSize: "40px", fontFamily: "Arial", transform: "r45," + totalResultTimePoint.x + "," + totalResultTimePoint.y });
            }

            for (var i: number = 0; i < profile.length; i++) {
                var dot: Snap.Element = paper.circle(profile[i].x, profile[i].y, 5);
                dot.attr({ fill: "#333333" });

                var prev: Point = profile[i - 1];
                var point: Point = profile[i];
                var next: Point = profile[i + 1];

                var textOffset: Vector = this.calcTextOffset(prev, point, next, 30);

                var textPoint: Point = profile[i].offset(textOffset);

                var text: Snap.Element = paper.text(textPoint.x, textPoint.y, i.toString());
                text.attr({ textAnchor: "middle", dominantBaseline: "middle", fill: "#333333", fontSize: 20 });
            }

            var color: string = "#a9a9a9";

            paper.path(super.toPathString(profile)).attr({ fill: "none", stroke: color, strokeWidth: 1.7, filter: "url(#penFilter)" }); // filter: "url(#penFilter)"

        }

        renderDistances(paper: Snap.Paper, data: ChartData, paperArea: Rectangle): void {
            var bottomLeftPoint: Point = new Point(paperArea.x, paperArea.y + paperArea.height);
            var bottomRightPoint: Point = new Point(paperArea.x + paperArea.width, paperArea.y + paperArea.height);

            Text.render(paper, data.distanceAxis.format(data.distanceAxis.min, true), bottomLeftPoint.offset(new Vector(20, -20)), Alignment.leftBottom, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.distanceAxis.format(data.distanceAxis.max, true), bottomRightPoint.offset(new Vector(-20, -20)), Alignment.rightBottom, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
        }


        calcTextOffset(prev: Point, point: Point, next: Point, distance: number): Vector {
            var direction: Vector;

            if (prev === undefined) {
                direction = Vector.createNormal(point, next);
            } else if (next === undefined) {
                direction = Vector.createNormal(prev, point);
            } else {
                var firstVector: Vector = Vector.createNormalized(prev, point);
                var secondVector: Vector = Vector.createNormalized(next, point);

                direction = firstVector.add(secondVector);
            }

            var vector: Vector = direction.normalize().scaleTo(distance);

            return vector;
        }

        renderSplits(paper: Snap.Paper, data: ChartData, profileArea: Rectangle): void {
        }

        render(profile: IProfile, result: IResult, width: number): void {
            var surfaceArea: Rectangle = this.surfaceArea;

            var paperArea: Rectangle = surfaceArea.apply(new Margin(10, 10, 10, 10));

            var widthMargin: number = paperArea.width / 20;
            var chartTopMargin: number = paperArea.height / 4 - 60;
            var chartBottomMargin: number = paperArea.height / 16;

            var headerArea: Rectangle = paperArea.apply(new Margin(0, 0, 0, paperArea.height - chartTopMargin));
            var chartArea: Rectangle = paperArea.apply(new Margin(widthMargin, chartTopMargin, widthMargin, chartBottomMargin));
            var profileArea: Rectangle = chartArea.apply(new Margin(0, 200, 0, 0));

            var data: ChartData = new ChartData(profile, result, profileArea);

            var height: number = (surfaceArea.height / surfaceArea.width) * width;

            var paper: Snap.Paper = Snap("#connectingDotsChart");
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
        }
        constructor() {
            super("57B271BD-CA75-42BD-B7FD-A5A0EBEC887F", "Connecting dots", "/App/ProfileChart/Templates/connecting_dots.svg");
        }
    }

    export class SimplySunshineChart extends Chart {

        constructor() {
            super("19930022-DDB3-4CFC-A75E-3E8CC2DEEB04", "Simply Sunshine", "/App/ProfileChart/Templates/simply_sunshine.svg");
        }

        renderBackground(paper: Snap.Paper, surfaceArea: Rectangle): void {
            var g: any = paper.gradient("r(0.5, 0.5, 1)#fff:0-#FBFDFF:30-#EEF8FD:43-#D9F0FB:57-#BBE4F9:71-#fff:25-#95D5F5:86-#67C3F1");

            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        }

        renderGrid(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {

            var source: Rectangle = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());

            var transform: TransformProcessor = new TransformProcessor(source, chartArea);

            paper.rect(chartArea.x, chartArea.y, chartArea.width, chartArea.height).attr({ stroke: "#A4A3A3", fill: "none"});

            for (var altitude: number = data.altitudeAxis.min; altitude <= data.altitudeAxis.max; altitude = altitude + data.altitudeAxis.gridMinor) {
                var altitudePoint: Point = transform.processPoint(new Point(0, altitude));

                paper.line(chartArea.x, altitudePoint.y, chartArea.x + chartArea.width, altitudePoint.y).attr({ stroke: "#A4A3A3" });

                var altitudeMajor: boolean = data.altitudeAxis.major(altitude);

                var altitudeLine: Snap.Element = paper.line(chartArea.x - 35, altitudePoint.y, chartArea.x - 20, altitudePoint.y);

                if (altitudeMajor) {
                    altitudeLine.attr({ stroke: "#231F20", strokeWidth: 2 });
                    Text.render(paper, data.altitudeAxis.format(altitude, false), new Point(chartArea.x - 45, altitudePoint.y), Alignment.rightMiddle, { fontSize: "24px", fill: "#515151", fontFamily: "Biko" });
                } else {
                    altitudeLine.attr({ stroke: "#A4A3A3", strokeWidth: 1 });
                }

            }

            for (var distance: number = data.distanceAxis.min; distance <= data.distanceAxis.max; distance = distance + data.distanceAxis.gridMinor) {
                var distancePoint: Point = transform.processPoint(new Point(distance, 0));

                paper.line(distancePoint.x, chartArea.y, distancePoint.x, chartArea.y + chartArea.height).attr({ stroke: "#A4A3A3" });

                var distanceMajor: boolean = data.distanceAxis.major(distance);

                var top: number = chartArea.y + chartArea.height;

                var distanceLine: Snap.Element = paper.line(distancePoint.x, top + 10, distancePoint.x, top + 20);

                if (distanceMajor) {
                    distanceLine.attr({ stroke: "#231F20", strokeWidth: 2 });
                    Text.render(paper, data.distanceAxis.format(distance, distance === data.distanceAxis.min), new Point(distancePoint.x, top + 30), Alignment.centerTop, { fontSize: "24px", fill: "#515151", fontFamily: "Biko" });
                } else {
                    distanceLine.attr({ stroke: "#A4A3A3", strokeWidth: 1 });
                }
            }



        }

        renderProfile(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {

            var reduce: PointProcessor = new ReduceToNumberProcessor(1000);

            var profile: Array<Point> = data.courseProfile;

            profile = reduce.process(profile);

            var profileBody: Point[] = [];

            profileBody.push(new Point(chartArea.x, profile[0].y));

            profileBody.push.apply(profileBody, profile);

            profileBody.push(new Point(chartArea.x + chartArea.width, profile[profile.length - 1].y));
            profileBody.push(new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, profile[0].y));

            var bodyPathString: string = super.toPathString(profileBody) + " Z";

            var g: any = paper.gradient("l(0.5, 1, 0.5, 0)#81F5E0-#56B5FB");

            paper.path(bodyPathString).attr({ fill: g, opacity: 0.8, stroke: "#105A77", strokeWidth: 2 });
        }

        renderPlace(paper: Snap.Paper, chartArea: Rectangle, location: Point, name: string, index: number): void {
            var width: number = 180;
            var height: number = 36;
            var signPadding: number = 5;

            var offsetSignY: number = 100;

            if (index % 2 === 1) {
                offsetSignY += height + signPadding;
            }

            var signPoint: Point = new Point(location.x, chartArea.y - offsetSignY);


            var text: Snap.Element = Text.render(paper, name, signPoint, Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });
            var textSize: Snap.BBox = text.getBBox();

            var cornerRadius: number = height / 2;
            width = textSize.width + cornerRadius * 2;


            var signSize: Offset = new Offset(width / 2, height / 2);

            var sign: Snap.Element = paper.rect(signPoint.x - signSize.width, signPoint.y - signSize.height + 5, width, height, cornerRadius, cornerRadius).attr({ fill: "#56C4CC" });
            text.before(sign);

            var points: Point[] = [];

            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x, signPoint.y + signSize.height + 14));
            points.push(new Point(signPoint.x + 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));


            text.before(paper.path(super.toPathString(points) + " Z").attr({ fill: "#56C4CC" }));


            paper.line(signPoint.x, signPoint.y + signSize.height + 14 + 10, location.x, location.y).attr({ fill: "none", stroke: "#000000", strokeWidth: 1, strokeDasharray: "5, 5" });;
        }

        renderPlaces(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {
            for (let i: number = 0; i < data.places.length; i++) {
                this.renderPlace(paper, chartArea, data.places[i].point, data.places[i].name, i);
            }
        }

        renderSplits(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {
            var source: Rectangle = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());

            var transform: TransformProcessor = new TransformProcessor(source, chartArea);

            var signHeight: number = 36;
            var signCornerRadius: number = signHeight / 2; 

            var offset: Offset = new Offset(0, -200);

            for (var i: number = 1; i < data.splits.length; i++) {
                var split: ChartSplit = data.splits[i];

                var splitPoint: Point = transform.processCoordinate(split.distance, 0);

                var placeText: Snap.Element = Text.render(paper, split.name, new Point(splitPoint.x, chartArea.y + offset.height), Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });
                var placeTextSize: Snap.BBox = placeText.getBBox();

                var signWidth: number = placeTextSize.width + signCornerRadius * 2;

                placeText.before(paper.rect(splitPoint.x - signWidth / 2, chartArea.y + offset.height - 18 + 5, signWidth, signHeight, signCornerRadius, signCornerRadius).attr({ fill: "#E03B3B" }));

                var splitTimeY: number = chartArea.y + offset.height - 24;

                var splitTimeText: Snap.Element = Text.render(paper, split.getTime(), new Point(splitPoint.x, splitTimeY), Alignment.centerBottom, { fill: "#333333", fontSize: "24px", fontWeight: "bold", fontFamily: "Arial" });
                var bbox: Snap.BBox = splitTimeText.getBBox();

                paper.el("use", { "xlink:href": "#stopwatch", x: bbox.x, y: splitTimeY + 12 });

                var radius: number = 5;
                paper.circle(splitPoint.x, chartArea.y + offset.height + 40, radius).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });
                paper.circle(splitPoint.x, chartArea.y + chartArea.height + radius, radius).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });

                paper.line(splitPoint.x, chartArea.y + offset.height + 40 + radius, splitPoint.x, chartArea.y + chartArea.height).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });

                if (i === data.splits.length - 1) {
                    paper.el("use", { "xlink:href": "#finish_flag", x: splitPoint.x, y: splitTimeY - 18 });
                }
            }
        }

        renderSunAndClouds(paper: Snap.Paper): void {
            paper.el("use", { "xlink:href": "#sun", x: 240, y: 100 });
            paper.el("use", { "xlink:href": "#clouds", x: 280, y: 130});
        }

        renderHeader(paper: Snap.Paper, data: ChartData, headerArea: Rectangle): void {
            var topCenterPoint: Point = new Point(headerArea.x + headerArea.width / 2, headerArea.y);

            var courseNamePoint: Point = topCenterPoint.offset(new Vector(0, 80));

            var courseNameText: Snap.Element = Text.render(paper, data.courseName, courseNamePoint, Alignment.centerBottom, { fill: "#E03B3B", fontSize: "72px", fontFamily: "Arial" });
            var bbox: Snap.BBox = courseNameText.getBBox();

            paper.line(bbox.x, bbox.y + bbox.height, bbox.x + bbox.width, bbox.y + bbox.height).attr({ fill: "none", stroke: "#56C4CC", strokeWidth: 6 });

            Text.render(paper, data.athlete.displayName, courseNamePoint, Alignment.centerTop, { fill: "#E03B3B", fontSize: "64px", fontFamily: "Arial" });
        }

        render(profile: IProfile, result: IResult, width: number): void {

            var surfaceArea: Rectangle = this.surfaceArea;

            var widthMargin: number = surfaceArea.width / 20;
            var heightMargin: number = surfaceArea.height / 2;

            var headerArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin));
            var chartArea: Rectangle = surfaceArea.apply(new Margin(widthMargin * 2, heightMargin, widthMargin, 80));
            // var profileArea: Rectangle = chartArea.apply(new Margin(0, chartArea.height / 4, 0, chartArea.height / 2));

            var data: ChartData = new ChartData(profile, result, chartArea);

            var height: number = (surfaceArea.height / surfaceArea.width) * width;

            var paper: Snap.Paper = Snap("#simplySunshineChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });

            this.clearChart();

            this.renderBackground(paper, this.surfaceArea);
            this.renderGrid(paper, data, chartArea);
            this.renderProfile(paper, data, chartArea);
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
        }
    }

    export class GiroItaliaChart extends Chart {

        constructor() {
            super("E6C5D286-BF69-4FD0-A6DE-F46ACC53F011", "Giro d'Italia", "/App/ProfileChart/Templates/giro_italia.svg");
        }

        renderBackground(paper: Snap.Paper, surfaceArea: Rectangle): void {
            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: '#FFFFFF' });
        }

        renderRulerScaleSegment(paper: Snap.Paper, from: Point, to: Point, rulerHeight: Vector, color: string): void {
            let points: Array<Point> = [];

            points.push(from);
            points.push(to);
            points.push(to.offset(rulerHeight));
            points.push(from.offset(rulerHeight));

            paper.path(super.toPathString(points) + " Z").attr({ fill: color, stroke: "#000000", strokeWidth: 2 });
        }

        renderDistanceRuler(paper: Snap.Paper, data: ChartData, chartArea: Rectangle, transform: PointProcessorPipeLine): void {

            var source: Rectangle = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());

            let rulerHeight: Vector = new Vector(0, -12);
            let lastMajorDistancePoint: Point = null; 
            let even: boolean = true;
            let color: string = "#000000";

            for (var distance: number = data.distanceAxis.min; distance <= data.distanceAxis.max; distance = distance + data.distanceAxis.gridMinor) {
                var distancePoint: Point = transform.processPoint(new Point(distance, 0));

                var distanceMajor: boolean = data.distanceAxis.major(distance);

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
                    var endPoint: Point = transform.processPoint(new Point(data.distanceAxis.max, 0));

                    color = even ? "#000000" : "#FFFFFF";

                    this.renderRulerScaleSegment(paper, lastMajorDistancePoint, endPoint, rulerHeight, color);
                }
            }
        }

        renderGround(paper: Snap.Paper, profile: Point[], offset: Vector, origo: Point): void {
            var color: string = "#A2A7AF";
            var ascendingColor: string = "#A2A7AF";
            var descandingColor: string = "#999999";

            var points: Point[] = [];

            points.push(origo);
            points.push(profile[0]);
            points.push(profile[0].offset(offset));
            points.push(origo.offset(offset));

            paper.path(super.toPathString(points) + " Z").attr({ fill: ascendingColor });

            for (var i: number = 1; i < profile.length; i++) {

                points = [];

                points.push(profile[i - 1]);
                points.push(profile[i - 1].offset(offset));
                points.push(profile[i].offset(offset));
                points.push(profile[i]);
                points.push(profile[i - 1]);

                if (profile[i - 1].y > profile[i].y) {
                    color = ascendingColor;
                } else {
                    color = descandingColor;
                }

                var pathString: string = super.toPathString(points) + " Z";
                paper.path(pathString).attr({ fill: color });
            }
        }

        renderLine(paper: Snap.Paper, from: Point, to: Point): Snap.Element {
            return paper.line(from.x, from.y, to.x, to.y);
        }

        renderProfile(paper: Snap.Paper, data: ChartData, chartArea: Rectangle, frontTransform: PointProcessorPipeLine): void {
            var frontOffset: Vector = new Vector(24, 14);

            var reduce: PointProcessor = new ReduceToNumberProcessor(100);
            var skew: PointProcessor = new SkewProcessor(new Vector(10, -1).normalize());
            var offset: PointProcessor = new OffsetProcessor(frontOffset);

            var profile: Array<Point> = reduce.process(data.courseProfile);


            var backProfile: Array<Point> = skew.process(profile);
            var backOrigo: Point = skew.processPoint(new Point(chartArea.x, chartArea.y + chartArea.height));

            var frontProfile: Array<Point> = offset.process(skew.process(profile));

            paper.path(super.toPathString(backProfile)).attr({ fill: 'none', stroke: "#000000", strokeWidth: 9, strokeLinejoin: "round" });

            this.renderGround(paper, backProfile, frontOffset, backOrigo);

            var lineAttr = { fill: 'none', stroke: "#000000", strokeWidth: 7 };

            //this.drawLine(paper, backProfile[backProfile.length - 1], frontProfile[frontProfile.length - 1]).attr(lineAttr);
            this.renderLine(paper, backProfile[0], frontProfile[0]).attr({ fill: 'none', stroke: "#000000", strokeWidth: 5 });
            this.renderLine(paper, backProfile[0], frontProfile[0]).attr({ fill: 'none', stroke: "#AAAAAA", strokeWidth: 2 });
            this.renderLine(paper, backProfile[0], backOrigo).attr(lineAttr);
            this.renderLine(paper, backOrigo, backOrigo.offset(frontOffset)).attr({ fill: 'none', stroke: "#000000", strokeWidth: 5 });


            var profileBody: Point[] = [];

            profileBody.push(new Point(chartArea.x, profile[0].y));

            profileBody.push.apply(profileBody, profile);

            profileBody.push(new Point(chartArea.x + chartArea.width, profile[profile.length - 1].y));
            profileBody.push(new Point(chartArea.x + chartArea.width, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, chartArea.y + chartArea.height));
            profileBody.push(new Point(chartArea.x, profile[0].y));


            var frontProfileBody: Array<Point> = offset.process(skew.process(profileBody));

            var g: any = paper.gradient("l(0.5, 1, 0.5, 0)#FCFDED:25-#D7E8BE:50-#EEF8FD:75-#EDCAA0:100-#F8FAF9");

            g.transform("r-22 0 0");

            var frontBodyPathString: string = super.toPathString(frontProfileBody) + " Z";

            paper.path(frontBodyPathString).attr({ fill: g, stroke: "#000000", strokeWidth: 4, strokeLinejoin: "round" });

            var frontProfilePathString: string = super.toPathString(frontProfile);

            paper.path(frontProfilePathString).attr({ fill: 'none', stroke: "#000000", strokeWidth: 9, strokeLinejoin: "round" });
            paper.path(frontProfilePathString).attr({ fill: 'none', stroke: "#CC2D3C", strokeWidth: 5, strokeLinejoin: "round" });

            this.renderDistanceRuler(paper, data, chartArea, frontTransform);
        }

        renderPlace(paper: Snap.Paper, chartArea: Rectangle, location: Point, name: string, index: number): void {
            var width: number = 180;
            var height: number = 36;
            var signPadding: number = 5;

            var offsetSignY: number = 100;

            if (index % 2 === 1) {
                offsetSignY += height + signPadding;
            }

            var signSize: Offset = new Offset(width / 2, height / 2);
            var signPoint: Point = new Point(location.x, chartArea.y - offsetSignY);

            paper.rect(signPoint.x - signSize.width, signPoint.y - signSize.height + 5, width, height, signSize.height, signSize.height).attr({ fill: "#56C4CC" });

            var points: Point[] = [];

            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x, signPoint.y + signSize.height + 14));
            points.push(new Point(signPoint.x + 10, signPoint.y + signSize.height));
            points.push(new Point(signPoint.x - 10, signPoint.y + signSize.height));

            paper.path(super.toPathString(points) + " Z").attr({ fill: "#56C4CC" });

            Text.render(paper, name, signPoint, Alignment.centerMiddle, { fill: "#FFFFFF", fontSize: "24px", fontFamily: "Arial" });

            paper.line(signPoint.x, signPoint.y + signSize.height + 14 + 10, location.x, location.y).attr({ fill: "none", stroke: "#000000", strokeWidth: 1, strokeDasharray: "5, 5" });;
        }

        renderPlaces(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {
            for (let i: number = 0; i < data.places.length; i++) {
                this.renderPlace(paper, chartArea, data.places[i].point, data.places[i].name, i);
            }
        }

        renderSplits(paper: Snap.Paper, data: ChartData, chartArea: Rectangle, backTransform: PointProcessorPipeLine): void {
            var source: Rectangle = new Rectangle(data.distanceAxis.min, data.altitudeAxis.min, data.distanceAxis.getSpan(), data.altitudeAxis.getSpan());

            var transform: TransformProcessor = new TransformProcessor(source, chartArea);

            var lineOffset: Vector = new Vector(0, -450);
            var textOffset: Vector = new Vector(0, -20);
             
            var startBasePoint = backTransform.processPoint(data.getFirstSplit().point);
            var startTopPoint = startBasePoint.offset(lineOffset);

            
            //var direction: Vector = new Vector(10, -1);
            //var directionPoint = startTopPoint.offset(direction.scaleToX(200));
            //this.renderLine(paper, startTopPoint, directionPoint).attr({ fill: "none", stroke: "#000000", strokeWidth: 1 });

            this.renderLine(paper, startBasePoint, startTopPoint).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });

            var startText: Snap.Element = Text.render(paper, data.getFirstSplit().altitude.toFixed(0) + " - " + data.getFirstSplit().name.toUpperCase(), startTopPoint.offset(textOffset), Alignment.leftBottom, { fontSize: "48px", fill: "#000000", fontFamily: "Arial" });
            startText.transform("r-5 " + startTopPoint.x + " " + startTopPoint.y);


            //Text.render(paper, data.getLastSplit().name, topRightPoint.offset(new Vector(-20, 20)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            //Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " m", topRightPoint.offset(new Vector(-20, 60)), Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });



            var offset: Offset = new Offset(0, -200);

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


            var finishBasePoint = backTransform.processPoint(data.getLastSplit().point);
            var finishTopPoint = finishBasePoint.offset(lineOffset);

            this.renderLine(paper, finishBasePoint, finishTopPoint).attr({ fill: "none", stroke: "#515151", strokeWidth: 3 });

            var finishText: Snap.Element = Text.render(paper, data.getLastSplit().altitude.toFixed(0) + " - " + data.getLastSplit().name.toUpperCase(), finishTopPoint.offset(textOffset), Alignment.rightBottom, { fontSize: "48px", fill: "#000000", fontFamily: "Arial" });
            finishText.transform("r-5 " + finishTopPoint.x + " " + finishTopPoint.y);

        }

        renderSunAndClouds(paper: Snap.Paper): void {
            paper.el("use", { "xlink:href": "#sun", x: 240, y: 100 });
            paper.el("use", { "xlink:href": "#clouds", x: 280, y: 130 });
        }

        renderHeader(paper: Snap.Paper, data: ChartData, headerArea: Rectangle): void {
            var topCenterPoint: Point = new Point(headerArea.x + headerArea.width / 2, headerArea.y);

            var courseNamePoint: Point = topCenterPoint.offset(new Vector(0, 80));

            var courseNameText: Snap.Element = Text.render(paper, data.courseName, courseNamePoint, Alignment.centerBottom, { fill: "#A2A7AF", fontSize: "72px", fontFamily: "Arial" });

//            var bbox: Snap.BBox = courseNameText.getBBox();

//            paper.line(bbox.x, bbox.y + bbox.height, bbox.x + bbox.width, bbox.y + bbox.height).attr({ fill: "none", stroke: "#56C4CC", strokeWidth: 6 });

            Text.render(paper, data.athlete.displayName, courseNamePoint, Alignment.centerTop, { fill: "#A2A7AF", fontSize: "64px", fontFamily: "Arial" });
        }

        render(profile: IProfile, result: IResult, width: number): void {

            var surfaceArea: Rectangle = this.surfaceArea;

            var widthMargin: number = surfaceArea.width / 20;
            var heightMargin: number = surfaceArea.height / 2;

            var headerArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin));
            var chartArea: Rectangle = surfaceArea.apply(new Margin(widthMargin * 2, heightMargin, widthMargin, 80));
            // var profileArea: Rectangle = chartArea.apply(new Margin(0, chartArea.height / 4, 0, chartArea.height / 2));

            var data: ChartData = new ChartData(profile, result, chartArea);

            var height: number = (surfaceArea.height / surfaceArea.width) * width;

            var paper: Snap.Paper = Snap("#giroItaliaChart");
            paper.attr({ width: width, height: height });
            paper.attr({ viewBox: surfaceArea.x + " " + surfaceArea.y + " " + surfaceArea.width + " " + surfaceArea.height });

            this.clearChart();

            this.renderBackground(paper, this.surfaceArea);

            var frontOffset: Vector = new Vector(24, 14);

            var reduce: PointProcessor = new ReduceToNumberProcessor(100);
            var skew: PointProcessor = new SkewProcessor(new Vector(10, -1).normalize());
            var offset: PointProcessor = new OffsetProcessor(frontOffset);

            var backTransform: PointProcessorPipeLine = new PointProcessorPipeLine();
//            backTransform.add(data.transform);
            backTransform.add(skew);

            var frontTransform: PointProcessorPipeLine = new PointProcessorPipeLine();
            frontTransform.add(data.transform);
            frontTransform.add(skew);
            frontTransform.add(offset);


            this.renderProfile(paper, data, chartArea, frontTransform);

            this.renderSplits(paper, data, chartArea, backTransform);
            //this.renderPlaces(paper, data, chartArea);
            //this.renderSunAndClouds(paper);
            this.renderHeader(paper, data, headerArea);

            // paper.circle(100, 200, 3);
            // paper.el("use", { "xlink:href": "#sun", x: 100, y: 200 });
            // paper.circle(200, 200, 3);
            // paper.el("use", { "xlink:href": "#clouds", x: 200, y: 200 });
            // paper.circle(300, 200, 3);
            // paper.el("use", { "xlink:href": "#finish_flag", x: 300, y: 200 });
            // paper.circle(400, 200, 3);
            // paper.el("use", { "xlink:href": "#stopwatch", x: 400, y: 200 });
        }
    }


    export class ForestChart extends Chart {

        private maxVisibleSplitResults: number = 7;

        private finishResultTimeOffset: Vector = new Vector(0, -210);
        private finishResultPositionOffset: Vector = new Vector(0, -210);

        private splitResultTimeOffset: Vector = new Vector(-50, -155);
        private splitResultPositionOffset: Vector = new Vector(140, -160);
        private splitResultLabelOffset: Vector = new Vector(-30, -120);

        private trackOffset: Vector = new Vector(0, 150);
        private stopwatchWidth: number = 20;

        renderBackground(paper: Snap.Paper, surfaceArea: Rectangle): void {
            var g: any = paper.gradient("l(0, 1, 1, 0)#29ABE2-#FFFFFF");

            paper.rect(surfaceArea.x, surfaceArea.y, surfaceArea.width, surfaceArea.height).attr({ fill: g });
        }

        renderHeader(paper: Snap.Paper, data: ChartData, area: Rectangle): void {

            var topLeftPoint: Point = new Point(data.getFirstSplit().point.x, area.y);
            var topRightPoint: Point = new Point(data.getLastSplit().point.x, area.y);

            Text.render(paper, data.splits[0].altitude.toFixed(0) + "m " + data.splits[0].name, topLeftPoint, Alignment.leftTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.getLastSplit().altitude.toFixed(0) + "m " + data.getLastSplit().name, topRightPoint, Alignment.rightTop, { fontSize: "32px", fill: "#676868", fontFamily: "Arial" });

            Text.render(paper, "+" + data.ascending.toFixed(0) + "m ", topRightPoint.offset(new Vector(0, 40)), Alignment.rightTop, { fontSize: "20px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, "-" + data.descending.toFixed(0) + "m ", topRightPoint.offset(new Vector(0, 60)), Alignment.rightTop, { fontSize: "20px", fill: "#676868", fontFamily: "Arial" });

            var topCenterPoint: Point = new Point(area.x + area.width / 2, area.y);

            // var courseNameText = paper.text(textPoint.x, textPoint.y, data.courseName);
            // courseNameText.attr({ fontSize: "48px", fill: "#2ba7de", fontFamily: "Arial" });

            var courseNamePoint: Point = topCenterPoint.offset(new Vector(0, 5));
            // var courseNameShadow = this.renderText(paper, data.courseName, courseNamePoint.offset(new Vector(-2, 2)), Alignment.centerTop, { fontSize: "48px", fill: "#676868", fontFamily: "Arial" });
            Text.render(paper, data.courseName, courseNamePoint, Alignment.centerTop, { fontSize: "48px", fill: "#676868", fontFamily: "Arial" });

            var name: string = data.athlete.displayName;
            var resultNamePoint: Point = topCenterPoint.offset(new Vector(0, 60));
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

            var resultTimePoint: Point = topCenterPoint.offset(new Vector(0, 120));
            Text.render(paper, new ElapsedTime(data.elapsedSeconds).toString(), resultTimePoint, Alignment.centerTop, { fontSize: "64px", fill: "#676868", fontFamily: "Arial" });
        }

        renderProfile(paper: Snap.Paper, data: ChartData, surfaceArea: Rectangle, profileArea: Rectangle): void {
            // var reduce = new ReduceProcessor(2, 1);
            var reduce: PointProcessor = new ReduceToNumberProcessor(30);

            var profile: Array<Point> = data.courseProfile;

            profile = reduce.process(profile);

            var offset: Vector = new Vector(5, -15);

            this.renderGround(paper, profile, offset, surfaceArea);

            // paper.path(super.toPathString(profile)).attr({ fill: "none", stroke: color, strokeWidth: 1 });
            // paper.path(super.toPathString(courseProfile)).attr({ fill: "none", stroke: "#FF0000", strokeWidth: 1 });        
            // from #bfdbf2 to #0294d8

            var profileBody: Point[] = [];

            profileBody.push(new Point(surfaceArea.x, profile[0].y));

            profileBody.push.apply(profileBody, profile);

            profileBody.push(new Point(surfaceArea.x + surfaceArea.width, profile[profile.length - 1].y));
            profileBody.push(new Point(surfaceArea.x + surfaceArea.width, surfaceArea.y + surfaceArea.height));
            profileBody.push(new Point(surfaceArea.x, surfaceArea.y + surfaceArea.height));
            profileBody.push(new Point(surfaceArea.x, profile[0].y));

            var bodyPathString: string = super.toPathString(profileBody) + " Z";

            var g: any = paper.gradient("l(0, 0.5, 1, 0.5)#bfdbf2-#0294d8");

            paper.path(bodyPathString).attr({ fill: g });

            var totalLength: number = 0.0;

            for (let i: number = 1; i < profile.length; i++) {
                totalLength += Vector.create(profile[i - 1], profile[i]).getLength();
            }

            for (let i: any = 1; i < profile.length; i++) {
                var length: number = Vector.create(profile[i - 1], profile[i]).getLength();

                this.renderTrees(paper, profile[i - 1], profile[i], offset, Math.floor(length * 60 / totalLength));
            }

            this.renderOwlTree(paper, profile);

            if (this.settings.renderSkierAndTrack) {
                this.renderSkierAndTrack(paper, profile);
            }

            paper.el("use", { "xlink:href": "#finish_flag", x: profile[profile.length - 1].x, y: profile[profile.length - 1].y });

            // paper.rect(profileArea.x, profileArea.y, profileArea.width, profileArea.height).attr({ fill: "none", stroke: "#FF0000" });
        }

        renderSkierAndTrack(paper: Snap.Paper, profile: Point[]): void {
            var trackReduce: PointProcessor = new ReduceToNumberProcessor(10);
            var offseter: OffsetProcessor = new OffsetProcessor(this.trackOffset);

            var trackPoints: Point[] = offseter.process(trackReduce.process(profile));

            trackPoints.unshift(offseter.processPoint(profile[0]));
            trackPoints.push(offseter.processPoint(profile[profile.length - 1]));

            // paper.path(super.toPathString(trackPoints)).attr({ fill: "none", stroke: "#000000", strokeWidth: 1 });
            paper.path(super.createCurveThroughPath(trackPoints)).attr({ fill: "none", stroke: "#000000", strokeWidth: 1 });

            offseter = new OffsetProcessor(new Vector(0, -2));
            trackPoints = offseter.process(trackPoints);

            paper.path(super.createCurveThroughPath(trackPoints)).attr({ fill: "none", stroke: "#FFFFFF", strokeWidth: 2 });

            paper.el("use", { "xlink:href": "#skier", x: trackPoints[0].x, y: trackPoints[0].y });
        }

        renderGround(paper: Snap.Paper, profile: Point[], offset: Vector, surfaceArea: Rectangle): void {
            var color: string = "#666666";
            var ascendingColor: string = "#808080";
            var descandingColor: string = "#999999";

            var points: Point[] = [];

            points.push(new Point(surfaceArea.x, profile[0].y));
            points.push(new Point(surfaceArea.x, profile[0].y + offset.y));
            points.push(profile[0].offset(offset));
            points.push(profile[0]);
            points.push(new Point(surfaceArea.x, profile[0].y));

            paper.path(super.toPathString(points) + " Z").attr({ fill: descandingColor });

            for (var i: number = 1; i < profile.length; i++) {

                points = [];

                points.push(profile[i - 1]);
                points.push(profile[i - 1].offset(offset));
                points.push(profile[i].offset(offset));
                points.push(profile[i]);
                points.push(profile[i - 1]);

                if (profile[i - 1].y > profile[i].y) {
                    color = ascendingColor;
                } else {
                    color = descandingColor;
                }

                var pathString: string = super.toPathString(points) + " Z";
                paper.path(pathString).attr({ fill: color });
            }

            points.push(profile[profile.length - 1]);
            points.push(new Point(surfaceArea.x + surfaceArea.width, profile[profile.length - 1].y));
            points.push(new Point(surfaceArea.x + surfaceArea.width, profile[profile.length - 1].y + offset.y));
            points.push(profile[profile.length - 1].offset(offset));
            points.push(profile[profile.length - 1]);

            paper.path(super.toPathString(points) + " Z").attr({ fill: descandingColor });
        }

        renderOwlTree(paper: Snap.Paper, profile: Point[]): void {
            var colors: string[] = ["#588427", "#6a992f", "#48711e", "#31550e", "#3d6317", "#1f4100", "#395f15", "#3a6015", "#32560f", "#77a935", "#365f16", "#173900"];

            var segment: number = Math.floor(Math.random() * (profile.length - 1));
            var from: Point = profile[segment];
            var to: Point = profile[segment];

            var point: Point = this.getRandomTreePoint(from, to, new Vector(0, 0));
            var color: string = colors[Math.floor(Math.random() * colors.length)];


            paper.el("use", { "xlink:href": "#tree4", x: point.x, y: point.y, fill: color });
            var owl: Snap.Element = paper.el("use", { "xlink:href": "#owl", x: point.x, y: point.y, opacity: 0 });
            var eyes: Snap.Element = paper.el("use", { "xlink:href": "#owl_eyes", x: point.x, y: point.y });

            eyes.hover((event: MouseEvent) => { this.show(owl); }, (event: MouseEvent) => { this.hide(owl); });
        }

        renderTrees(paper: Snap.Paper, from: Point, to: Point, offset: Vector, nrOfTrees: number): void {
            var colors: string[] = ["#588427", "#6a992f", "#48711e", "#31550e", "#3d6317", "#1f4100", "#395f15", "#3a6015", "#32560f", "#77a935", "#365f16", "#173900"];

            for (var i: number = 0; i < nrOfTrees; i++) {
                var treeId: string = "#tree" + (Math.floor(Math.random() * 4) + 1);
                var point: Point = this.getRandomTreePoint(from, to, offset);
                var color: string = colors[Math.floor(Math.random() * colors.length)];
                var scaleX: number = 0.6 + Math.random() * 0.4;
                var scaleY: number = 0.7 + Math.random() * 0.3;

                var transform: string = "scale(" + scaleX + ", " + scaleY + ", " + point.x + "," + point.y + ")"; // translate(" + (point.x * -1/scaleX) + ", " + (point.y * 1/scaleY) + ")";

                paper.el("use", { "xlink:href": treeId, transform: transform, x: point.x / scaleX, y: point.y / scaleY, fill: color });
            }
        }

        getRandomTreePoint(from: Point, to: Point, offset: Vector): Point {
            var firstvector: Vector = Vector.create(from, to).scaleTo(Math.random());

            var secondvector: Vector = offset.scaleTo(Math.random());

            var vector: Vector = firstvector.add(secondvector);

            return from.offset(vector);
        }


        showEventHandler(event: MouseEvent): Function {
            return () => {
                // var el = <Snap.Element>event.srcElement;
                // el.attr({ opacity: "1" });
            };
        }

        hideEventHandler(event: MouseEvent): Function {
            return () => {
                // el.animate({ opacity: "0" }, 1000);
            };
        }

        show(el: Snap.Element): Function {
            return () => {
                el.attr({ opacity: "1" });
            };
        }

        hide(el: Snap.Element): Function {
            return () => {
                el.animate({ opacity: "0" }, 1000);
            };
        }

        renderSplits(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {
            // var start = new Point(data.splits[0].point.x, chartArea.y + chartArea.height);
            // var finish = new Point(data.splits[data.splits.length - 1].point.x, chartArea.y + chartArea.height)

            paper.line(data.splits[0].point.x, chartArea.y + chartArea.height, data.splits[data.splits.length - 1].point.x, chartArea.y + chartArea.height).attr({ stroke: "#555555", strokeWidth: 1 });

            var rigthBottom: Vector = new Vector(6, 0);
            var top: Vector = new Vector(0, -10);
            var leftBottom: Vector = new Vector(-6, 0);

            var distanceFontFalily: string = "Arial";

            for (var i: number = 0; i < data.splits.length; i++) {
                var split: ChartSplit = data.splits[i];

                paper.line(data.splits[i].point.x, data.splits[i].point.y, data.splits[i].point.x, chartArea.y + chartArea.height).attr({ stroke: "#555555", strokeWidth: 1, opacity: 0.65 });

                var circle: Snap.Element = null;

                if (i > 0) {
                    circle = paper.circle(data.splits[i].point.x, data.splits[i].point.y, 6).attr({ fill: "#FFFFFF", stroke: "#000000", strokeWidth: 1 });
                }

                var points: Point[] = [];

                var centerBottom: Point = new Point(data.splits[i].point.x, chartArea.y + chartArea.height);

                points.push(centerBottom);
                points.push(centerBottom.offset(rigthBottom));
                points.push(centerBottom.offset(top));
                points.push(centerBottom.offset(leftBottom));
                points.push(centerBottom);

                paper.path(super.toPathString(points) + "Z").attr({ fill: "#FFFFFF", stroke: "#555555", strokeWidth: 1 });

                var s: Snap.Element = paper.text(centerBottom.x, centerBottom.y, data.distanceAxis.format(split.distance, i === data.splits.length - 1)).attr({ fontFamily: distanceFontFalily, textAnchor: "middle", fontSize: 26, fontStyle: "bold", fill: "#676868" });

                var bbox: Snap.BBox = s.getBBox();
                s.attr({ y: centerBottom.y + bbox.height });

                if (i > 0 && i < data.splits.length - 1) {

                    var splitResultSymbol: Snap.Element = paper.el("use", { id: "split" + i, "xlink:href": "#result_split", x: data.splits[i].point.x, y: data.splits[i].point.y });

                    var splitResultTimePoint: Point = data.splits[i].point.offset(this.splitResultTimeOffset);

                    var splitTime: Snap.Element = paper.text(splitResultTimePoint.x, splitResultTimePoint.y, data.splits[i].getTime()).attr({ fill: "#F9F4F4", fontSize: 28, fontFamily: "Arial", fontStyle: "italic" });
                    this.align(splitTime, Alignment.leftMiddle, splitResultTimePoint);

                    // var splitTimeBox = splitTime.getBBox();
                    // splitTime.attr({ y: splitResultTimePoint.y + splitTimeBox.height / 2 });

                    var splitResultPositionPoint: Point = data.splits[i].point.offset(this.splitResultPositionOffset);

                    var splitPosition: Snap.Element = paper.text(splitResultPositionPoint.x, splitResultPositionPoint.y, data.splits[i].position.toString()).attr({ fill: "#676868", textAnchor: "middle", fontSize: "48px", fontFamily: "Arial", fontStyle: "bold" });
                    var splitPositionBox: Snap.BBox = splitPosition.getBBox();
                    splitPosition.attr({ y: splitResultPositionPoint.y + splitPositionBox.height / 2 });

                    var splitResultLabelPoint: Point = data.splits[i].point.offset(this.splitResultLabelOffset);
                    var splitLabel: Snap.Element = paper.text(splitResultLabelPoint.x, splitResultLabelPoint.y, data.splits[i].name).attr({ fill: "#676868", fontSize: "18px", fontFamily: "Arial" });
                    this.align(splitLabel, Alignment.centerMiddle, splitResultLabelPoint);

                    var splitResultGroup: any = paper.group(splitResultSymbol, splitTime, splitPosition, splitLabel);

                    var hide: boolean = data.splits.length > this.maxVisibleSplitResults;

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
        }

        //renderText(paper: Snap.Paper, text: string, position: Point, alignment: Alignment, params: Object): Snap.Element {
        //    var textElement = paper.text(position.x, position.y, text).attr(params);
        //    this.align(textElement, alignment, position);

        //    return textElement;
        //}

        renderLegs(paper: Snap.Paper, data: ChartData, chartArea: Rectangle): void {
            for (var i: number = 0; i < data.legs.length; i++) {
                var leg: ChartLeg = data.legs[i];

                var legResultOffset: Vector = this.trackOffset.scaleTo(0.5);
                var legMiddlePoint: Point = leg.middlepoint;
                var legResultPoint: Point = legMiddlePoint.offset(legResultOffset);

                //   paper.circle(legResultPoint.x, legResultPoint.y, 3);

                var stopwatch: Snap.Element = paper.el("use", { "xlink:href": "#stopwatch", x: legResultPoint.x, y: legResultPoint.y });

                var legResultTimePoint: Point = legResultPoint.offset(new Vector(this.stopwatchWidth, 0));

                var legTime: Snap.Element = Text.render(paper, leg.getTime(), legResultTimePoint, Alignment.leftBottom, { fill: "#F9F4F4", fontSize: 16, fontFamily: "Arial" });

                var legPosition: Snap.Element = Text.render(paper, "position " + leg.position, legResultPoint, Alignment.leftTop, { fill: "#676868", fontSize: 16, fontFamily: "Arial" });

                var legResult: any = paper.group(stopwatch, legTime, legPosition);
                var offset: Vector = this.calcAlignmentVector(legResult, HorizontalAlignment.Center, VerticalAlignment.Middle);
                var transform: string = "translate(" + offset.x + "," + -offset.y / 2 + ")";

                legResult.attr({ transform: transform });

                var hide = data.splits.length > this.maxVisibleSplitResults;

                if (hide) {
                    legResult.attr({ opacity: 0 });

                    var circle = paper.circle(legMiddlePoint.x, legMiddlePoint.y, 3).attr({ fill: "#FFFFFF", stroke: "#000000", strokeWidth: 1 });

                    circle.hover((event: MouseEvent) => { this.show(legResult) }, (event: MouseEvent) => { this.hide(legResult) });
                }
 
   
                //this.align(legResult, Alignment.centerMiddle, legResultPoint);
            }
        }

        render(profile: IProfile, result: IResult, width: number) {

            var surfaceArea = this.surfaceArea;

            var widthMargin = surfaceArea.width / 20;
            var heightMargin = surfaceArea.height / 3;

            var headerArea: Rectangle = surfaceArea.apply(new Margin(widthMargin, 0, widthMargin, heightMargin * 2));
            var chartArea: Rectangle = surfaceArea.apply(new Margin(widthMargin * 2, heightMargin, widthMargin, 40));
            var profileArea: Rectangle = chartArea.apply(new Margin(0, 120, 0, 120));

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
        }

        constructor(private settings: ChartRenderingSettings) {
            super(settings.chartId, settings.name, "/App/ProfileChart/Templates/forest.svg");
        }

    }

    export class ChartRenderingSettings {
        constructor(public chartId: string, public name: string, public renderPersonProfile: Boolean, public renderResultPositios: Boolean, public renderSkierAndTrack: Boolean)
        { }
    }

    class ChartProfile {
        transform: TransformProcessor;

        tpPoint(trackpoint: ITrackpoint): Point {
            return this.transform.processPoint(new Point(trackpoint.distance, trackpoint.altitude));
        }

        constructor(public points: Point[], target: Rectangle) {
            var extent: Extent = new Extent([new Point(0, 0)]);

            extent.containPoints(points);

            var source: Rectangle = extent.toRectangle();

            this.transform = new TransformProcessor(source, target);

            this.points = this.transform.process(points);
        }
    }

    class TrackpointConverter {
        toPoint(trackpoint: ITrackpoint): Point {
            return this.pipeline.processPoint(new Point(trackpoint.distance, trackpoint.altitude));
        }

        constructor(public pipeline: PointProcessorPipeLine) {
        }
    }

    export class ChartData {
        courseName: string;
        courseProfile: Point[];
        courseProfilePath: string;

        athlete: IAthlete;

        places: ChartPlace[];

        splits: ChartSplit[];
        legs: ChartLeg[];

        elapsedSeconds: number;
        position: number;

        ascending: number;
        descending: number;

        public distanceAxis: DistanceAxis;
        public altitudeAxis: AltitudeAxis;
        public profileExtent: Rectangle;

        public transform: TransformProcessor;
        
        getFirstSplit(): ChartSplit {
            return this.splits[0];
        }

        getLastSplit(): ChartSplit {
            return this.splits[this.splits.length - 1];
        }

        constructor(profile: IProfile, result: IResult, target: Rectangle) {
            console.log(JSON.stringify(result));
            this.courseName = profile.name;
            this.athlete = result.athlete;

            this.splits = [];
            this.legs = [];
            this.courseProfile = [];

            this.ascending = 0.0;
            this.descending = 0.0;

            for (var j = 0; j < profile.track.points.length; j++) {
                this.courseProfile.push(new Point(profile.track.points[j].distance, profile.track.points[j].altitude));
            }

            var extent: Extent = new Extent(this.courseProfile);

            this.profileExtent = extent.toRectangle();

            this.distanceAxis = new DistanceAxis(0, profile.track.length);
            this.altitudeAxis = new AltitudeAxis(extent.minY, extent.maxY);


            var pipeline = new PointProcessorPipeLine();

            var source: Rectangle = new Rectangle(this.distanceAxis.min, this.altitudeAxis.min, this.distanceAxis.getSpan(), this.altitudeAxis.getSpan());

            this.transform = new TransformProcessor(source, target);
            pipeline.add(this.transform);

            this.courseProfile = pipeline.process(this.courseProfile);

            var converter: TrackpointConverter = new TrackpointConverter(pipeline);

            this.places = new Array<ChartPlace>();
            var splitPlaces: Array<IProfilePlace> = new Array<IProfilePlace>();

            for (let i = 0; i < profile.places.length; i++) {
                if (profile.places[i].split)
                    splitPlaces.push(profile.places[i]);
                else
                    this.places.push(new ChartPlace(profile.places[i].place.name, profile.places[i].point.distance, profile.places[i].point.altitude, converter.toPoint(profile.places[i].point)));
            }

            for (let i: number = 0; i < splitPlaces.length; i++) {
                this.splits.push(new ChartSplit(splitPlaces[i].place.name, splitPlaces[i].point.distance, splitPlaces[i].point.altitude, converter.toPoint(splitPlaces[i].point), result.splits[i]));
            }

            for (let i: number = 0; i < profile.legs.length; i++) {
                var leg: ILeg = profile.legs[i];

                this.legs.push(new ChartLeg(converter.toPoint(leg.startPoint), converter.toPoint(leg.middlePoint), converter.toPoint(leg.endPoint), leg.length, leg.ascending, leg.descending));

                this.ascending += leg.ascending;
                this.descending += leg.descending;
            }

            this.elapsedSeconds = this.getLastSplit().elapsedSeconds;
            this.position = this.getLastSplit().position;
        }

    }

    export class ChartPlace {
        constructor(public name: string, public distance: number, public altitude: number, public point: Point) {
        }
    }


    export class ChartSplit extends ChartPlace {
        public elapsedSeconds: number;
        public position: number;

        getTime(): string {
            var elapsedTime: ElapsedTime = new ElapsedTime(this.elapsedSeconds);

            return elapsedTime.toString();
        }

        constructor(public name: string, public distance: number, public altitude: number, public point: Point, resultSplit: IResultSplit) {
            super(name, distance, altitude, point);

            this.position = resultSplit.totalPosition;
            this.elapsedSeconds = resultSplit.totalTimeSeconds;
        }
    }

    export class ChartLeg {
        elapsedSeconds: number;
        position: number;

        getTime(): string {
            return new ElapsedTime(this.elapsedSeconds).toString();
        }

        constructor(public startpoint: Point, public middlepoint: Point, public endpoint: Point, public length: number, public ascending: number, public descending: number) {
        }
    }

    class ChartAxis {
        calcGridBigFactor(gridFactor: number): number {

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
        }

        calcGridFactor(remainderMax: number): number {

            if (remainderMax > 5) {
                return 5;
            }

            if (remainderMax > 2) {
                return 2;
            }

            return 1;
        }

        getSpan(): number {
            return this.max - this.min;
        }

        public major(altitude: number): boolean {
            return (altitude % this.gridMajor) === 0;
        }

        constructor(public min: number, public max: number, public gridMinor: number, public gridMajor: number) {
        }


    }

    export class DistanceUnit {
        static meter: DistanceUnit = new DistanceUnit(1, "m");
        static kilometer: DistanceUnit = new DistanceUnit(1000, "km");

        constructor(public value: number, public suffix: string) {
        }
    }

    //enum DistanceUnit {
    //    Meter = 1,
    //    Kilometer = 1000
    //}

    export class DistanceAxis extends ChartAxis {
        public unit: DistanceUnit;
        public decimals: number;
        public roundValue: number;

        format(distance: number, withSuffix: Boolean): string {
            distance = distance / this.unit.value;
            distance = Math.floor(distance / this.roundValue) * this.roundValue;

            var res: string = parseFloat(distance.toFixed(this.decimals)).toString();

            //for(var i = res.length - 1; i >= 0; i--)
            // if(res[i] == "0")

            if (withSuffix)
                res += " " + this.unit.suffix;

            return res;
        }

        constructor(min: number, max: number) {

            var log10Max: number = Math.floor(Math.log(max - min) / Math.LN10);
            var remainderMax: number = Math.floor((max - min) / Math.pow(10, log10Max));

            var unit = log10Max < 3 ? DistanceUnit.meter : DistanceUnit.kilometer;

            var log10Unit = unit === DistanceUnit.kilometer ? 3 : 0;

            var log10Values = log10Max - log10Unit - 2;

            var gridFactor = super.calcGridFactor(remainderMax);

            var gridMinor = Math.pow(10, Math.max(log10Max - 1, 0)) * gridFactor;
            var gridMajor = Math.pow(10, Math.max(log10Max - 1, 0)) * super.calcGridBigFactor(gridFactor);

            super(min, max, gridMinor, gridMajor);

            this.unit = unit;
            this.roundValue = Math.pow(10, log10Values);
            this.decimals = log10Values < 0 ? Math.abs(log10Values) : 0;
        }
    }

    export class AltitudeAxis extends ChartAxis {
        public unit: number;
        public decimals: number;
        public roundValue: number;

        format(altitude: number, withSuffix: Boolean): string {
            altitude = Math.floor(altitude / this.roundValue) * this.roundValue;

            var res: string = parseFloat(altitude.toFixed(this.decimals)).toString();

            if (withSuffix)
                res += " m";

            return res;
        }

        private getRoundFactor(altitudeSpan: number, gridMinor: number, gridMajor: number): number {
            var roundFactor: number = 5;

            if (altitudeSpan > 20)
                roundFactor = gridMajor * 5;

            if (altitudeSpan > 50)
                roundFactor = gridMajor * 2;

            if (altitudeSpan > 100)
                roundFactor = gridMajor;

            if (altitudeSpan > 500)
                roundFactor = gridMinor;

            return roundFactor;
        }

        private getPadding(altitudeSpan: number): number {

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
        }

        private static calcMin(minAltitude: number, maxAltitude: number, gridMinor: number, gridMajor: number): number {
            var roundFactor: number = gridMajor;
            var minBase: number = Math.min(minAltitude, 0);
            
            return Math.max(Math.floor(minAltitude / roundFactor) * roundFactor, minBase);
        }

        private static calcMax(minAltitude: number, maxAltitude: number, gridMinor: number, gridMajor: number): number {
            var roundFactor: number = gridMajor;

            return Math.ceil(maxAltitude / roundFactor) * roundFactor;
        }

        constructor(minAltitude: number, maxAltitude: number) {
            var padding: number = 5000 / (maxAltitude - minAltitude);

            var minBase: number = Math.min(minAltitude, 0);

            minAltitude = Math.max(minAltitude - 3 * padding, minBase);
            maxAltitude = maxAltitude + padding;

            var altitudeSpan: number = maxAltitude - minAltitude;

            var log10Max: number = Math.floor(Math.log(altitudeSpan) / Math.LN10);
            var remainderMax: number = Math.floor(altitudeSpan / Math.pow(10, log10Max));

            var log10Unit = 0;

            var log10Values = log10Max - log10Unit - 2;
            var log10Grid = Math.max(log10Max - 1, 0);

            var gridFactor = super.calcGridFactor(remainderMax);

            var gridMinor = Math.pow(10, log10Grid) * gridFactor;
            var gridMajor = Math.pow(10, log10Grid) * super.calcGridBigFactor(gridFactor);

            var min = AltitudeAxis.calcMin(minAltitude, maxAltitude, gridMinor, gridMajor);
            var max = AltitudeAxis.calcMax(minAltitude, maxAltitude, gridMinor, gridMajor);

            super(min, max, gridMinor, gridMajor);

            this.unit = 1;
            this.roundValue = Math.pow(10, log10Values);
            this.decimals = 0;
        }
    }


    class PointProcessor {
        process(points: Point[]): Point[] {
            return null;
        }

        processPoint(point: Point): Point {
            return null;
        }

        constructor(public name: string) { }
    }

    class PointProcessorPipeLine extends PointProcessor {
        private processors: Array<PointProcessor> = new Array<PointProcessor>();

        add(processor: PointProcessor): PointProcessorPipeLine {
            this.processors.push(processor);

            return this;
        }

        processPoint(point: Point): Point {
            return this.process([point])[0];
        }

        process(points: Point[]) {

            for (var i = 0; i < this.processors.length; i++)
                points = this.processors[i].process(points);

            return points;
        }

        constructor() {
            super("pipeline");
        }

        //constructor(processors: PointProcessor[])
        //{
        //    super("pipeline");

        //    for (var i = 0; i < processors.length; i++)
        //        this.processors.push(processors[i]);
        //}
    }

    class TransformProcessor extends PointProcessor {
        processCoordinate(x: number, y: number): Point {
            return this.processPoint(new Point(x, y));
        }

        processPoint(point: Point): Point {
            return this.process([point])[0];
        }

        process(points: Point[]): Point[] {
            var result: Point[] = [];

            var scaleX = this.target.width / this.source.width;
            var scaleY = this.source.height > 0 ? this.target.height / this.source.height : 1;

            for (var i = 0; i < points.length; i++) {
                var x = this.target.x + (points[i].x - this.source.x) * scaleX;
                var y = this.target.y + this.target.height - (points[i].y - this.source.y) * scaleY;

                result.push(new Point(x, y));
            }

            return result;
        }

        constructor(private source: Rectangle, private target: Rectangle) {
            super("Transform");
        }
    }

    class ReduceProcessor extends PointProcessor {
        private static maxLevels: number = 1000;
        private static minEpsilon: number = 0.00001;

        process(points: Point[]): Point[] {
            return this.douglasPeucker(points, 0);
        }

        calcPerpendicularDistance(location: Point, start: Point, end: Point) {
            var a: number = location.x - start.x;
            var b: number = location.y - start.y;
            var c: number = end.x - start.x;
            var d: number = end.y - start.y;

            return Math.abs(a * d - c * b) / Math.sqrt(c * c + d * d);
        }

        calcHeightDistance(point: Point, start: Point, end: Point): number {
            var k: number = (end.y - start.y) / (end.x - start.x);

            var y: number = start.y + k * (point.x - start.x);

            return Math.abs(point.y - y);
        }


        douglasPeucker(points: Point[], level: number): Point[] {
            var maxDistance: number = 0.0;
            var maxAltitudeIndex: number = 0;

            for (var i = 1; i < points.length - 2; i++) {
                //var distance: number = this.calcPerpendicularDistance(points[i], points[0], points[points.length - 1]);
                var distance: number = this.calcHeightDistance(points[i], points[0], points[points.length - 1]);

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
        }

        constructor(private maxlevels: number, private epsilon: number) {
            super("Reduce");
        }
    }

    class ReduceToNumberProcessor extends PointProcessor {

        process(points: Point[]): Point[] {

            if (points.length <= this.nrOfPoints)
                return points;

            var rootSegment = new ReduceSegment(points);

            for (var i = 0; i < this.nrOfPoints; i++) {
                var segment = this.findSegmentWithMaxDistance(rootSegment);

                segment.split();
            }

            var result: Point[] = [];

            result.push(rootSegment.points[0]);
            result.push.apply(result, this.getPoints(rootSegment));
            result.push(rootSegment.points[rootSegment.points.length - 1]);

            return result;
        }

        getPoints(segment: ReduceSegment): Array<Point> {
            var points: Array<Point> = new Array<Point>();

            if (!segment.splitted)
                return points;

            points.push.apply(points, this.getPoints(segment.firstSubSegment));
            points.push(segment.maxDistancePoint);
            points.push.apply(points, this.getPoints(segment.secondSubSegment));

            return points;
        }

        findSegmentWithMaxDistance(segment: ReduceSegment): ReduceSegment {
            if (!segment.splitted)
                return segment;

            var firstSegment = this.findSegmentWithMaxDistance(segment.firstSubSegment);
            var secondSegment = this.findSegmentWithMaxDistance(segment.secondSubSegment);

            if (firstSegment.maxDistance >= secondSegment.maxDistance)
                return firstSegment;

            return secondSegment;
        }

        constructor(private nrOfPoints: number) {
            super("ReducePointsSegment");
        }
    }

    class ReduceSegment {
        public maxDistance: number = 0.0;
        public maxDistanceIndex: number = 0.0;
        public maxDistancePoint: Point = null;

        public splitted: Boolean = false;
        public firstSubSegment: ReduceSegment = null;
        public secondSubSegment: ReduceSegment = null;

        split() {
            this.firstSubSegment = new ReduceSegment(this.points.slice(0, this.maxDistanceIndex + 1));
            this.secondSubSegment = new ReduceSegment(this.points.slice(this.maxDistanceIndex, this.points.length));

            this.splitted = true;
        }

        constructor(public points: Point[]) {
            for (var i = 1; i < points.length - 1; i++) {
                var distance: number = points[i].heightDistanceTo(points[0], points[points.length - 1]);

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

    }

    class OffsetProcessor extends PointProcessor {

        process(points: Point[]): Point[] {
            var offsetedPoints: Point[] = [];

            for (var i = 0; i < points.length; i++) {
                offsetedPoints.push(points[i].offset(this.offset));
            }

            return offsetedPoints;
        }

        processPoint(point: Point): Point {
            return point.offset(this.offset);
        }

        constructor(private offset: Vector) {
            super("Offset");
        }
    }

    class SkewProcessor extends PointProcessor {
        process(points: Point[]): Point[] {
            var skewPoints: Point[] = [];

            for (var i = 0; i < points.length; i++) {
                skewPoints.push(this.processPoint(points[i]));
            }

            return skewPoints;
        }

        processPoint(point: Point): Point {
            return new Point(point.x, point.y + this.vector.scaleToX(point.x).y);
        }

        constructor(private vector: Vector) {
            super("Skew");
        }
    }


    export class MountainGenerator {
        public points: Array<Point> = new Array<Point>();

        constructor(private rectangle: Rectangle, levels: number, numberOfPoints: number) {
            var start: Point = new Point(rectangle.x, rectangle.y + rectangle.height * Math.random());
            var end: Point = new Point(rectangle.x + rectangle.width, rectangle.y + rectangle.height * Math.random());

            this.generatePoints(numberOfPoints, start, end, rectangle.height / 2, levels);
        }

        generatePoints(numberOfPoints: number, start: Point, end: Point, spanY: number, level: number) {

            var vector: Vector = Vector.create(start, end);
            var totalWidth = end.x - start.x;

            var segmentWidth = totalWidth / numberOfPoints;


            this.points.push(start);

            var lastPoint: Point = start; 
            
            for (let i = 0; i < numberOfPoints; i++) {
                var dx = i * segmentWidth + segmentWidth * Math.random();
                var dy = vector.scaleTo(dx / totalWidth).y;

                var point: Point = new Point(start.x + dx, start.y + dy + spanY * Math.random() - spanY / 2); 

                if (level > 0)
                    this.generatePoints(numberOfPoints, lastPoint, point, spanY / 6, level - 1);

                this.points.push(point);

                lastPoint = point;
            }    

            if (level > 0)
                this.generatePoints(numberOfPoints, lastPoint, end, spanY / 6, level - 1);

            this.points.push(end);
        }

    }

    export interface IDistanceCalculator {
        calculate(start: Point, point: Point, end: Point): number;
    }

    export interface IAthlete {
        displayName: string;
    }

    interface IPerson {
        firstName: string;
        lastName: string;
        displayName: string;
        startNumber: string;
        club: string;
        company: string;
        country: string;
    }


    export interface IProfilePoint {
        distance: number;
        altitude: number;
    }

    export interface IProfilePlace {
        place: IPlace;
        point: ITrackpoint;
        active: Boolean;
        split: Boolean;
    }

    export interface IPlace {
        name: string;
    }

    export interface IProfile {
        name: string;
        track: ITrack;
        places: IProfilePlace[];
        legs: ILeg[];
    }

    export interface ITrack {
        length: number;
        points: ITrackpoint[];
        firstPoint: ITrackpoint;
        lastPoint: ITrackpoint;
    }

    export interface ILeg {
        startPoint: ITrackpoint;
        middlePoint: ITrackpoint;
        endPoint: ITrackpoint;
        length: number;
        ascending: number;
        descending: number;
    }

    export interface ITrackpoint {
        latitude: number;
        longitude: number;
        altitude: number;
        distance: number;
    }

    export interface IResult {
        athlete: IAthlete;

        startTime: Date;
        totalTimeSeconds: number;

        splits: IResultSplit[];
    }

    export interface IResultOld {
        firstName: string;
        lastName: string;
        displayName: string;
        startNumber: string;
        club: string;
        company: string;
        country: string;

        startTime: Date;
        totalTimeSeconds: number;

        splits: IResultSplit[];
    }


    export interface IResultSplit {
        time: Date;
        legTimeSeconds: number;
        legPosition: number;
        totalTimeSeconds: number;
        totalPosition: number;
    }

    export interface IChartType {
        id: string;
        name: string;
        unlockOnLevel: number;
        isUnlocked: boolean;
    }
}