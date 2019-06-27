using System;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Collections.Specialized;
using System.Linq;

namespace TextSummary
{
    class TextSummaryContent
    {
        public string text;
        public int originalSeqNumber;
        public int senPriority;
        public int senPrioritySeq;
        public TextSummaryContent(string text, int originalSeqNumber, int senPriority, int senPrioritySeq)
        {
            this.text = text;
            this.originalSeqNumber = originalSeqNumber;
            this.senPriority = senPriority;
            this.senPrioritySeq = senPrioritySeq;
        }
    }

    class TextSummaryConvertor
    {
        string text;
        string originalSeqNumber;
        string senPriority;
        string senPrioritySeq;

        List<TextSummaryContent> SentenceObjList = new List<TextSummaryContent>();

        string[] priority1 = {"critical","production","live","incident","severity","emergency","visible","action","assigned","finish",
             "talk","contact","send","complete","finish","implemented"};

        string[] priority2 = { "defect", "fix", "status", "important", "success" };

        string[] priority3 = { "done", "completed", "work", "task", "release" };

        int Totalsentences = 0;
        int linecount = 0;
        public TextSummaryConvertor()
        {

        }

        public void processFile()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\PiyuNir\hackerton-project\SampleText.txt");
            string lowerCaseLine = "";
            int importance = 0;
            foreach (string line in lines)
            {
                lowerCaseLine = "";
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
                lowerCaseLine = line.ToLower();
                importance = 0;

                foreach (string data in priority1)
                {
                    if (lowerCaseLine.Contains(data))
                    {
                        importance += 21;
                    }
                }

                foreach (string data in priority2)
                {
                    if (lowerCaseLine.Contains(data))
                    {
                        importance += 13;
                    }
                }

                foreach (string data in priority3)
                {
                    if (lowerCaseLine.Contains(data))
                    {
                        importance += 8;
                    }
                }
                linecount += 1;
                TextSummaryContent content = new TextSummaryContent(line, linecount, importance, 0);
                SentenceObjList.Add(content);
            }

            Totalsentences = linecount;
            var orderedSentenceByPri = SentenceObjList.OrderBy(x => x.senPriority).ToArray();
            int Sequence = Totalsentences;

            foreach (TextSummaryContent content in orderedSentenceByPri)
            {
                content.senPrioritySeq = Sequence;
                Sequence -= 1;
            }
            var orderedSentenceBySeq = SentenceObjList.OrderBy(x => x.originalSeqNumber).ToArray();
            /* 20 % weightage */
            int PrioritiesToDisplay = Totalsentences - (Totalsentences - (Totalsentences * 20) / 100);

            /* Display Summary Text */
            Console.WriteLine(" = *80 ");
            Console.WriteLine("*** Mintues of Meeting ****");

            foreach (TextSummaryContent content in SentenceObjList)
            {
                if (content.senPrioritySeq <= PrioritiesToDisplay) {
                    Console.WriteLine(content.text);
                }
            }
        }
    }
}