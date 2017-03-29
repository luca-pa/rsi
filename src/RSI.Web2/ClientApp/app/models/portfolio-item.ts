export class PortfolioItem {
    public data: string
    public ticker: string
    public prezzoAcquisto: string
    public quantita: string
    public isin: string
    public url: string
    public nome: string
    public importoAcquisto: string
    public prezzoCorrente: string
    public importoCorrenteNetto: string
    public guadagno: string
    public guadagnoPercentuale: string
    public variazione: string
    public dataVendita: string
    public prezzoVendita: string
    public dividendi: string

    public constructor(init?: Partial<PortfolioItem>) {
        Object.assign(this, init);
    }
}