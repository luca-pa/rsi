create procedure PerformanceByDate as
select a.Data, (valore-5-(Invested-Cash))/(Invested-Cash)*100--, valore, Invested 
from (
	select qp.Data, sum(Chiusura*Quantita) valore
	from QuotePortafoglio qp join Portafoglio p on qp.Ticker = p.Ticker
	where (qp.Data between p.Data and dateadd(DAY, -1, p.DataVendita))
	or (p.DataVendita is null and qp.Data >= p.Data)
	group by qp.Data
) a 
join Bilancio b on b.Data = (select max(Data) from Bilancio where Data <= a.Data)
order by a.Data

go

--select qp.Data, qp.Ticker, sum(Chiusura*Quantita) valore
--from QuotePortafoglio qp join Portafoglio p on qp.Ticker = p.Ticker
--where (qp.Data between p.Data and dateadd(DAY, -1, p.DataVendita))
--or (p.DataVendita is null and qp.Data >= dateadd(DAY, -1, p.Data))
--group by qp.Data, qp.Ticker
--order by qp.Data

--select * from Portafoglio order by 2