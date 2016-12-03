module AltCore {
    export class Vector {

        static combineNormalized(first: Vector, second: Vector): Vector {

            var vector = new Vector(first.x + second.x, first.y + second.y);

            var dotProduct = first.x * second.x + first.y * second.y;

            var scale = 1 / (dotProduct + 1);


            return new Vector(vector.x * scale, vector.y * scale);
        }



        normalize(): Vector {
            var length = Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));

            return new Vector(this.x / length, this.y / length);
        }



        scaleTo(scale: number): Vector {
            return new Vector(this.x * scale, this.y * scale);
        }

        scale(scaleX: number, scaleY: number): Vector {
            return new Vector(this.x * scaleX, this.y * scaleY);
        }

        add(vector: Vector): Vector {
            return new Vector(this.x + vector.x, this.y + vector.y);
        }

        Reverse(): Vector {
            return new Vector(-this.x, -this.y);
        }

        getLength(): number {
            return Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));
        }
        //static CalcCrossProduct(first: Vector, second: Vector): Vector {
        //    return new Vector(first.x * second.y) - (first.y * second.y);
        //}
        
        constructor(public x: number, public y: number) {
        }
    }

}