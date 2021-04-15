using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGameRule
{
    class Program
    {
        private static readonly int _totalCount = 15;// 总数
        private static readonly List<int> _totalLineSteps = new List<int>() { 3, 5, 7 };// 初始布局
        private static List<int> _lineSteps = new List<int>();// 当前布局
        private static readonly List<GamePlayer> _gamePlayers = new List<GamePlayer>() { new GamePlayer("玩家A", 1), new GamePlayer("玩家B", 2) };// 玩家

        static void Main(string[] args)
        {
            PrintInfo("游戏开始");
            PrintInfo("");

            _lineSteps.AddRange(_totalLineSteps);

            int round = 1;
            bool flag = true;
            while (flag)
            {
                PrintInfo($"第{round}轮游戏：");

                for (int i = 0; i < _gamePlayers.Count; i++)
                {
                    var resultData = GetLineStep();
                    int line = resultData.Item1;
                    int step = resultData.Item2;
                    _lineSteps = _gamePlayers[i].AddStep(line, step, _lineSteps);

                    PrintInfo($"{_gamePlayers[i].Name}从第{line}行拿走了{step}根。还剩下的数据依次为{GetEachStep()}");

                    if (_lineSteps.Sum() == 0)
                    {
                        flag = false;
                        PrintInfo($"游戏结束，{_gamePlayers[i].Name}拿到了最后一根即为输家");
                        break;
                    }
                }
                PrintInfo("");
                round++;
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 获取当前布局
        /// </summary>
        /// <returns></returns>
        private static String GetEachStep()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var lineStep in _lineSteps)
                sb.AppendFormat($"{lineStep}、");
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// 模拟每次从哪行拿走几根
        /// </summary>
        /// <returns>第几行第几根</returns>
        private static Tuple<int, int> GetLineStep()
        {
            int line = new Random().Next(1, _totalLineSteps.Count);
            int maxStep = GetMaxStep(line);
            if (maxStep < 1)
            {
                return LoopLineStep(line);
            }
            int step = new Random().Next(1, GetMaxStep(line));
            return new Tuple<int, int>(line, step);
        }

        /// <summary>
        /// 模拟每次从哪行拿走几根
        /// </summary>
        /// <param name="unLine">没有剩余的行</param>
        /// <returns>第几行第几根</returns>
        private static Tuple<int, int> LoopLineStep(int unLine)
        {
            int line = 0;
            for (int i = 0; i < _lineSteps.Count; i++)
            {
                if (unLine - 1 == i || GetMaxStep(i + 1) < 1) continue;

                line = i + 1;
            }
            int step = new Random().Next(1, GetMaxStep(line));
            return new Tuple<int, int>(line, step);
        }

        /// <summary>
        /// 获取每行当前可以取得的最大值
        /// </summary>
        /// <param name="line">当前行</param>
        /// <returns></returns>
        private static int GetMaxStep(int line)
        {
            return _lineSteps[line - 1];
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="info">消息</param>
        private static void PrintInfo(string info)
        {
            Console.WriteLine(info);
        }
    }

    /// <summary>
    /// 玩家
    /// </summary>
    public class GamePlayer
    {
        public GamePlayer(string name, int level)
        {
            this.Name = name;
            this.Level = level;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 玩家序号
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 玩家进行一次游戏
        /// </summary>
        /// <param name="line">选择第几行</param>
        /// <param name="step">选择的步长</param>
        /// <param name="lineSteps">当前布局</param>
        /// <returns>玩家进行一次游戏后的布局</returns>
        public List<int> AddStep(int line, int step, List<int> lineSteps)
        {
            var lis = new List<int>();
            lis.AddRange(lineSteps);
            lis[line - 1] -= step;
            return lis;
        }
    }


}
