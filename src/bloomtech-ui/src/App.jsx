import { useEffect, useState } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import PriceChart from './PriceChart';

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
        const stockRes = await axios.get(`/api/Stock/history/${symbol}`);
        setStockData(stockRes.data);

        // 2. Haberleri Çek
        const newsRes = await axios.get(`/api/News/${symbol}`);
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
    
    // Her 1 dakikada bir sayfayı yenilemeden veriyi güncelle
    const interval = setInterval(fetchData, 60000);
    return () => clearInterval(interval);

  }, []);

  // --- BURASI YENİ: HESAPLAMALAR ---
  // Eski "latestPrice" kısmını sildik, yerine bunu koyduk.
  const currentData = stockData.length > 0 ? stockData[0] : {};
  const prevData = stockData.length > 1 ? stockData[1] : {};
  
  // Fiyat yoksa (null ise) 0 kabul et
  const currentPrice = currentData.price || 0;
  const prevPrice = prevData.price || currentPrice;

  const priceChange = currentPrice - prevPrice;
  const percentChange = prevPrice !== 0 ? (priceChange / prevPrice) * 100 : 0;
  
  // Rengi belirle (Pozitifse Yeşil, Negatifse Kırmızı)
  const trendColor = priceChange >= 0 ? "text-neon-green" : "text-neon-red";


  return (
    <div className="container-fluid p-4">
      {/* --- ÜST BAŞLIK --- */}
      <div className="d-flex justify-content-between align-items-center mb-4 border-bottom border-secondary pb-2">
        <h1 className="text-neon-green m-0">BLOOMTECH // TERMINAL <span style={{fontSize:'0.5em'}}>v1.0</span></h1>
        <div className="text-end">
           <h2 className={`m-0 ${trendColor}`}>
             MRNA: ${currentPrice.toFixed(2)}
           </h2>
           <small className="text-muted">MODERNA INC.</small>
        </div>
      </div>
      
      {/* --- YENİ EKLENEN KISIM: İSTATİSTİK ŞERİDİ --- */}
      {stockData.length > 0 && (
        <div className="row mb-4">
          <div className="col-12">
            <div className="terminal-card p-3 d-flex justify-content-around align-items-center text-center">
              
              {/* 1. KUTU: GÜNLÜK DEĞİŞİM */}
              <div>
                <small className="text-muted d-block">DAILY CHANGE</small>
                <span className={`fs-5 fw-bold ${trendColor}`}>
                  {priceChange >= 0 ? "+" : ""}{priceChange.toFixed(2)} ({percentChange.toFixed(2)}%)
                </span>
              </div>

              {/* Dikey Çizgi */}
              <div className="border-end border-secondary" style={{height: '40px'}}></div>

              {/* 2. KUTU: GÜNÜN ARALIĞI (High/Low) */}
              <div>
                <small className="text-muted d-block">DAY RANGE (L - H)</small>
                <span className="text-white fs-5 font-monospace">
                  {currentData.low?.toFixed(2)} - {currentData.high?.toFixed(2)}
                </span>
              </div>

              <div className="border-end border-secondary" style={{height: '40px'}}></div>

              {/* 3. KUTU: AÇILIŞ (Open) */}
              <div>
                <small className="text-muted d-block">OPEN</small>
                <span className="text-white fs-5 font-monospace">
                  {currentData.open?.toFixed(2)}
                </span>
              </div>

              <div className="border-end border-secondary" style={{height: '40px'}}></div>

              {/* 4. KUTU: HACİM (Volume) */}
              <div>
                <small className="text-muted d-block">VOLUME</small>
                <span className="text-info fs-5 font-monospace">
                  {(currentData.volume / 1000000).toFixed(2)}M
                </span>
              </div>

            </div>
          </div>
        </div>
      )}
      {/* -------------------------------------------------- */}
      {/* --- GRAFİK BÖLÜMÜ ---*/}
      {stockData.length > 0 && (
        <div className="row mb-4">
          <div className="col-12">
            {/* Veriyi (stockData) PriceChart bileşenine gönderiyoruz */}
            <PriceChart data={stockData} />
          </div>
        </div>
      )}

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
                    <td className="text-neon-green font-monospace">
                      {new Date(trade.transactionDate).toLocaleDateString()}
                    </td>
                    <td className="text-white">
                      {trade.url ? (
                        <a 
                          href={trade.url} 
                          target="_blank" 
                          rel="noreferrer" 
                          className="text-white text-decoration-none hover-effect"
                          style={{borderBottom: '1px dotted #666'}}
                        >
                          {trade.name} <span style={{fontSize: '0.7em', color: '#00ff00'}}>↗</span>
                        </a>
                      ) : (
                        trade.name
                      )}
                    </td>
                    <td className="text-info">
                       {trade.type}
                    </td>
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