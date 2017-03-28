import { PortfolioItem } from './portfolio-item';

export interface Portfolio {
    items: PortfolioItem[]
    totaleAcquisto: string
    totaleCorrente: string
    guadagno: string
    guadagnoPercentuale: string
    guadagnoTotale: string
    guadagnoTotalePercentuale: string
    totaleComplessivo: string
    traderLinkUrl: string
    totaleInvestito: string
    cash: string
    minusvalenze: string
    dataBilancio: string
    variazione: string
}