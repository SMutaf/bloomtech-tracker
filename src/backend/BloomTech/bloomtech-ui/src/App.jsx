import { useEffect, useState } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

function App() {
  // Verileri tutacak "Kutular" (State)
  const [stockData, setStockData] = useState([]);
  const [newsData, setNewsData] = useState([]);
  const [insiderData, setInsiderData] = useState([]);
  const [loading, setLoading] = useState(true);

  // Sayfa açılınca çalışacak kod
  useEffect(() => {
    const fetchData = async () => {
      try {
        const symbol = "MRNA";
        
        // 1. Hisse Fiyatlarını Çek
        // (Vite Proxy sayesinde /api yazınca 7136'ya gider)
        const stockRes = await axios.get(`/api/Stock/history/${symbol}`);
        setStockData(stockRes.data);

        // 2. Haberleri Çek
        // (Eğer endpoint adın farklıysa burayı düzelt: /api/News/...)
        const newsRes = await axios.get(`/api/News/${symbol}`); // Endpoint adını kontrol et
        setNewsData(newsRes.data);

        // 3. Insider İşlemlerini Çek
        const insiderRes = await axios.get(`/api/Insider/${symbol}`);
        setInsiderData(insiderRes.data);

        setLoading(false);
      } catch (error) {
        console.error("Veri çekme hatası:", error);
        setLoading(false);
      }
    };

    fetchData();
    
    // Her 1 dakikada bir sayfayı yenilemeden veriyi güncelle (Otomatik Refresh)
    const interval = setInterval(fetchData, 60000);
    return () => clearInterval(interval);

  }, []);

  // Son fiyatı bul
  const latestPrice = stockData.length > 0 ? stockData[0].price : 0;
  const previousPrice = stockData.length > 1 ? stockData[1].price : latestPrice;
  const isUp = latestPrice >= previousPrice;

  return (
    <div className="container-fluid p-4">
      <div className="d-flex justify-content-between align-items-center mb-4 border-bottom border-secondary pb-2">
        <h1 className="text-neon-green m-0">BLOOMTECH // TERMINAL <span style={{fontSize:'0.5em'}}>v1.0</span></h1>
        <div className="text-end">
           <h2 className={isUp ? "text-neon-green m-0" : "text-neon-red m-0"}>
             MRNA: ${latestPrice.toFixed(2)}
           </h2>
           <small className="text-muted">MODERNA INC.</small>
        </div>
      </div>
      
      <div className="row">
        {/* SOL KOLON: Haberler */}
        <div className="col-md-6">
          <div className="terminal-card p-3 mb-3" style={{height: '400px', overflowY: 'auto'}}>
            <h5 className="border-bottom border-secondary pb-2">NEWS WIRE (RSS)</h5>
            {newsData.length === 0 ? <p>Haber yok...</p> : (
              <ul className="list-unstyled">
                {newsData.map((news, index) => (
                  <li key={index} className="mb-3">
                    <a href={news.url} target="_blank" rel="noreferrer" className="text-decoration-none text-white d-block hover-effect">
                      <span className="text-neon-green">[{new Date(news.publishedDate).toLocaleDateString()}]</span> {news.title}
                    </a>
                    <small className="text-muted">{news.source}</small>
                  </li>
                ))}
              </ul>
            )}
          </div>
        </div>

        {/* SAĞ KOLON: Insider Trades */}
        <div className="col-md-6">
          <div className="terminal-card p-3 mb-3" style={{height: '400px', overflowY: 'auto'}}>
            <h5 className="border-bottom border-secondary pb-2">SEC INSIDER FILINGS</h5>
            <table className="table table-dark table-sm table-hover" style={{fontSize: '0.9rem'}}>
              <thead>
                <tr>
                  <th className="text-muted">Date</th>
                  <th className="text-muted">Person</th>
                  <th className="text-muted">Form</th>
                </tr>
              </thead>
              <tbody>
                {insiderData.map((trade, index) => (
                  <tr key={index}>
                    <td className="text-neon-green">{new Date(trade.transactionDate).toLocaleDateString()}</td>
                    <td>{trade.name}</td>
                    <td>{trade.type}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  )
}

export default App;