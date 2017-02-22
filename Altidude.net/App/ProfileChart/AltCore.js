var AltCore;
(function (AltCore) {
    var Vector = (function () {
        //static CalcCrossProduct(first: Vector, second: Vector): Vector {
        //    return new Vector(first.x * second.y) - (first.y * second.y);
        //}
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
        Vector.prototype.scale = function (scaleX, scaleY) {
            return new Vector(this.x * scaleX, this.y * scaleY);
        };
        Vector.prototype.add = function (vector) {
            return new Vector(this.x + vector.x, this.y + vector.y);
        };
        Vector.prototype.Reverse = function () {
            return new Vector(-this.x, -this.y);
        };
        Vector.prototype.getLength = function () {
            return Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));
        };
        return Vector;
    }());
    AltCore.Vector = Vector;
})(AltCore || (AltCore = {}));
//# sourceMappingURL=AltCore.js.map