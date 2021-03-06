﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Tokenattributes;
using PanGu;

namespace WitBird.Com.SearchEngine
{
    public class SplitContent
    {
        public static string[] SplitWords(string content)
        {
            List<string> strList = new List<string>();
            //指定使用盘古 PanGuAnalyzer 分词算法
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(content));
            ITermAttribute term = tokenStream.AddAttribute<ITermAttribute>();
            ITermAttribute tempTerm = null; 
            while (tokenStream.IncrementToken())
            { 
                //Next继续分词 直至返回null
                //得到分词后结果
                if (tokenStream.HasAttribute<ITermAttribute>())
                {
                    tempTerm = tokenStream.GetAttribute<ITermAttribute>();
                    strList.Add(tempTerm.Term); 
                }
            }
            return strList.ToArray();
        }

        //需要添加PanGu.HighLight.dll的引用
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        public static string HightLight(string keyword, string content)
        {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                new PanGu.HighLight.SimpleHTMLFormatter("<font style=\"font-style:normal;color:#cc0000;\"><b>", "</b></font>");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
                            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 300;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
    }
}
