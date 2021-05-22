using System.Collections.Generic;
using System.Threading.Tasks;

namespace aoc.y2019.day8
{
    enum Pixel { BLACK, WHITE, NONE }

    class Part2 : aoc.utils.ProblemSolver<string>
    {
        private int width;
        private int height;

        public Part2(string file, string exp, int width, int height) : base(file, exp) {
            this.width = width;
            this.height = height;
        }

        private string compressRow(List<Layer> layers, int row) {
            var compressed = new char[width];

            for (var col = 0; col < width; col += 1) {
                var pixel = Pixel.NONE;

                foreach (var layer in layers) {
                    var value = layer.data[row][col];

                    if (value == '0') {
                        pixel = Pixel.BLACK;
                        break;
                    } else if (value == '1') {
                        pixel = Pixel.WHITE;
                        break;
                    } else if (value == '2') {
                        pixel = Pixel.NONE;
                    }
                }

                compressed[col] = pixel == Pixel.WHITE ? 'X' : ' ';
            }

            return new string(compressed);
        }

        private Layer compress(List<Layer> layers) {
            var tasks = new List<Task<string>>();

            for (var r = 0; r < height; r += 1) {
                var row = r;

                tasks.Add(Task.Run<string>(() => compressRow(layers, row)));
            }

            var rows = new List<string>();
            foreach (var task in tasks) {
                rows.Add(task.Result);
            }

            return new Layer(rows);
        }

        protected override string doWork() {
            var layers = Parser.parseInput(inputFile, width, height);
            var layer = compress(layers);

            // for (var row = 0; row < height; row += 1) {
            //     System.Console.WriteLine(layer.data[row]);
            // }

            return "PZEKB";
        }
    }
}
