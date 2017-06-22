using BtVideo.Models;
using BtVideo.Models.Site;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace BtVideo.Helpers
{
    public class IndexManager
    {
        public static readonly IndexManager bookIndex = new IndexManager();
        public static readonly string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
        private IndexManager()
        {
        }
        //请求队列 解决索引目录同时操作的并发问题
        private Queue<MovieViewModel> bookQueue = new Queue<MovieViewModel>();
        /// <summary>
        /// 新增Books表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Add(Movie books)
        {
            MovieViewModel bvm = new MovieViewModel();
            bvm.MovieTitle = books.MovieTitle;
            bvm.MovieID = books.MovieID;
            bvm.IT = IndexType.Insert;
            bvm.MovieContent = books.MovieContent;
            bvm.Director = books.Director;
            bvm.Stars = books.Stars;
            bvm.PictureFile = books.PictureFile;
            bvm.Grade = books.Grade;

            bookQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 删除Books表信息时 添加删除索引请求至队列
        /// </summary>
        /// <param name="bid"></param>
        public void Del(int bid)
        {
            MovieViewModel bvm = new MovieViewModel();
            bvm.MovieID = bid;
            bvm.IT = IndexType.Delete;

            bookQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 修改Books表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Mod(Movie books)
        {
            MovieViewModel bvm = new MovieViewModel();
            bvm.MovieID = books.MovieID;
            bvm.MovieTitle = books.MovieTitle;
            bvm.IT = IndexType.Modify;
            bvm.MovieContent = books.MovieContent;
            bvm.Stars = books.Stars;
            bvm.Director = books.Director;
            bvm.PictureFile = books.PictureFile;
            bvm.Grade = books.Grade;

            bookQueue.Enqueue(bvm);
        }

        public void StartNewThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private void QueueToIndex(object para)
        {
            while (true)
            {
                if (bookQueue.Count > 0)
                {
                    CRUDIndex();
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }
        /// <summary>
        /// 更新索引库操作
        /// </summary>
        private void CRUDIndex()
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if (isExist)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);

            while (bookQueue.Count > 0)
            {
                Document document = new Document();
                MovieViewModel book = bookQueue.Dequeue();
                if (book.IT == IndexType.Insert)
                {
                    document.Add(new Field("id", book.MovieID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.MovieTitle, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.MovieContent, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("stars", book.Stars, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("director", book.Director, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("picfile", book.PictureFile, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("grade", book.Grade.ToString(), Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));

                    writer.AddDocument(document);
                }
                else if (book.IT == IndexType.Delete)
                {
                    writer.DeleteDocuments(new Term("id", book.MovieID.ToString()));
                }
                else if (book.IT == IndexType.Modify)
                {
                    //先删除 再新增
                    writer.DeleteDocuments(new Term("id", book.MovieID.ToString()));

                    document.Add(new Field("id", book.MovieID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.MovieTitle, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.MovieContent, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("stars", book.Stars, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("director", book.Director, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("picfile", book.PictureFile, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("grade", book.Grade.ToString(), Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));

                    writer.AddDocument(document);
                }
            }
            writer.Close();
            directory.Close();
        }
    }

    public class SearchHelper
    {

        public IEnumerable<Movie> Search(string keywords, int? page, out int count)
        {
            string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);

            //--------------------------------------这里配置搜索条件
            //PhraseQuery query = new PhraseQuery();
            //foreach(string word in Common.SplitContent.SplitWords(Request.QueryString["SearchKey"])) {
            //    query.Add(new Term("content", word));//这里是 and关系
            //}
            //query.SetSlop(100);

            //关键词Or关系设置
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery query = null;
            foreach (string word in SplitWords(keywords))
            {
                query = new TermQuery(new Term("content", word));
                queryOr.Add(query, Occur.SHOULD);//这里设置 条件为Or关系
                query = new TermQuery(new Term("title", word));
                queryOr.Add(query, Occur.SHOULD);//这里设置 条件为Or关系
            }
            //--------------------------------------
            TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
            //searcher.Search(query, null, collector);
            searcher.Search(queryOr, null, collector);

            int start = 0, end = 24;
            page = page ?? 1;
            
            ScoreDoc[] docs = collector.TopDocs(start * page.Value, end).ScoreDocs;//取前十条数据  可以通过它实现LuceneNet搜索结果分页

            List<Movie> bookResult = new List<Movie>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].Doc;
                Document doc = searcher.Doc(docId);

                Movie book = new Movie();
                book.MovieTitle = doc.Get("title");
                book.MovieContent = HightLight(keywords, doc.Get("content"));
                book.MovieID = Convert.ToInt32(doc.Get("id"));
                book.Stars = doc.Get("stars");
                book.Director = doc.Get("director");
                book.PictureFile = doc.Get("picfile");
                book.Grade = double.Parse(doc.Get("grade"));

                bookResult.Add(book);
            }

            return bookResult;
        }

        private string[] SplitWords(string content)
        {
            List<string> strList = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();//指定使用盘古 PanGuAnalyzer 分词算法

            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(content));
            //Lucene.Net.Analysis.Token token = null;
            while (tokenStream.IncrementToken())
            { //Next继续分词 直至返回null
                strList.Add(tokenStream.GetAttribute<ITermAttribute>().Term); //得到分词后结果
            }
            return strList.ToArray();
        }

        //需要添加PanGu.HighLight.dll的引用
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        private string HightLight(string keyword, string content)
        {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                new PanGu.HighLight.SimpleHTMLFormatter("<font style=\"font-style:normal;color:#cc0000;\"><b>", "</b></font>");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
                            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 1000;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
    }
}