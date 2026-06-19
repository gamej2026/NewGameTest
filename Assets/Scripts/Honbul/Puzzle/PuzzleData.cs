using UnityEngine;

namespace Honbul
{
    public static class PuzzleData
    {
        public static MemoryNodeData[] LetterFragments()
        {
            return new[]
            {
                new MemoryNodeData(
                    0,
                    0,
                    "봄의 마당",
                    "수아야, 네가 세 살이던 봄에 마당 매화가 처음 폈단다.\n작은 손으로 꽃잎을 주워 내 주머니에 넣어 주던 날,\n나는 세상에서 가장 부유한 사람이 되었지.",
                    new Vector3(-2.8f, 1.1f, 0.2f)),
                new MemoryNodeData(
                    1,
                    1,
                    "장마의 우산",
                    "초등학교 첫 장마날, 네 우산이 뒤집혀 둘이 흠뻑 젖었지.\n너는 울다가도 내 손등을 닦아 주며 웃었고,\n나는 그 웃음 하나로 긴 비를 견뎠단다.",
                    new Vector3(-1.3f, 1.45f, 1.9f)),
                new MemoryNodeData(
                    2,
                    2,
                    "가을의 역",
                    "중학교 가던 해, 역 플랫폼에서 네가 말했지.\n\"할머니, 나 꼭 돌아올게.\"\n작은 여행가의 뒷모습을 보며 나는 손을 오래 흔들었단다.",
                    new Vector3(0f, 1.7f, 2.6f)),
                new MemoryNodeData(
                    3,
                    3,
                    "겨울의 침묵",
                    "도시가 너를 바쁘게 만들고, 전화는 점점 짧아졌지.\n안부를 묻는 문장마다 지운 흔적이 남아\n편지는 서랍 속에서 겨울처럼 쌓여만 갔단다.",
                    new Vector3(1.5f, 1.45f, 1.8f)),
                new MemoryNodeData(
                    4,
                    4,
                    "끝내 못 보낸 봉투",
                    "이 편지를 다 쓰면 부치려 했지만,\n혹시 짐이 될까 두려워 우체통 앞에서 매번 돌아섰다.\n그래도 알아다오. 나는 늘 네 편이었고, 지금도 그렇단다.",
                    new Vector3(2.9f, 1.1f, 0.1f))
            };
        }

        public static string[] StoryOnComplete()
        {
            return new[]
            {
                "흩어진 문장들이 한 줄의 마음으로 이어진다.",
                "봉투는 마침내 닫히고, 주소 없는 그리움도 길을 찾는다.",
                "할머니의 편지는 늦었지만, 네 이름 앞에서 가장 먼저 도착했다.",
                "전하지 못한 사랑이 이제 너의 오늘을 조용히 안아 준다."
            };
        }

        public static string ObjectiveExplore()
        {
            return "골목을 살피고 기억의 흔적을 찾아라";
        }

        public static string ObjectiveCollect(int got, int total)
        {
            return "기억 조각 수집: " + got + " / " + total;
        }

        public static string ObjectiveConnect()
        {
            return "혼실을 이어 편지의 시간을 완성하라";
        }
    }
}
