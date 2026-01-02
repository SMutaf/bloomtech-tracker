import React from 'react';
import { AreaChart, Area, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid } from 'recharts';

// --- DÜZELTME: Bu bileşeni dışarı taşıdık ---
const CustomTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    return (
      <div className="terminal-card p-2" style={{ border: '1px solid #00ff00', backgroundColor: '#000' }}>
        <p className="text-muted m-0" style={{ fontSize: '0.8rem' }}>{label}</p>
        <p className="text-neon-green m-0 fw-bold" style={{ fontSize: '1rem' }}>
          ${payload[0].value.toFixed(2)}
        </p>
      </div>
    );
  }
  return null;
};
// ---------------------------------------------

const PriceChart = ({ data }) => {
  // Backend'den veri "Yeniden -> Eskiye" geliyor.
  // Grafikte "Eskiden -> Yeniye" çizmek için diziyi kopyalayıp ters çeviriyoruz (reverse).
  const chartData = [...data].reverse().map(item => ({
    time: new Date(item.timestamp).toLocaleDateString('tr-TR'), 
    price: item.price
  }));

  return (
    <div className="terminal-card p-3 mb-4" style={{ height: '350px' }}>
      <h5 className="border-bottom border-secondary pb-2 mb-3 text-white">
        MARKET TREND <span className="text-neon-green" style={{fontSize: '0.6em'}}>● 1 YEAR HISTORY</span>
      </h5>
      
      <ResponsiveContainer width="100%" height="85%">
        <AreaChart data={chartData}>
          <defs>
            <linearGradient id="colorPrice" x1="0" y1="0" x2="0" y2="1">
              <stop offset="5%" stopColor="#00ff00" stopOpacity={0.3}/>
              <stop offset="95%" stopColor="#00ff00" stopOpacity={0}/>
            </linearGradient>
          </defs>
          <CartesianGrid strokeDasharray="3 3" stroke="#222" vertical={false} />
          
          <XAxis 
            dataKey="time" 
            stroke="#666" 
            tick={{fontSize: 10}} 
            minTickGap={30} 
          />
          
          <YAxis 
            domain={['auto', 'auto']} 
            stroke="#666" 
            tick={{fontSize: 12}} 
            orientation="right" 
            tickFormatter={(number) => `$${number.toFixed(0)}`}
          />
          
          <Tooltip content={<CustomTooltip />} />
          
          <Area 
            type="monotone" 
            dataKey="price" 
            stroke="#00ff00" 
            strokeWidth={2} 
            fillOpacity={1} 
            fill="url(#colorPrice)" 
          />
        </AreaChart>
      </ResponsiveContainer>
    </div>
  );
};

export default PriceChart;