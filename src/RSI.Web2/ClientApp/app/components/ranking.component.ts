import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Etf } from '../models/etf';
import { TradingService } from '../services/trading.service';
import { DateService } from '../services/date.service';

@Component({
    selector: 'ranking',
    templateUrl: './ranking.component.html',
    styleUrls: ['./ranking.component.css']
})
export class RankingComponent implements OnInit {
    public etfs: Etf[];
    public loading: boolean = true;
    public selection: boolean = true;
    public all: boolean = false;
    public tabellaTraderLinkUrl: string;
    public dataRif: Date;
    public dataPrecedente: Date;
    public dataSuccessiva: Date;
    public quoteAggiornate: string;
    public quoteAggiornateMeseSuccessivo: string;
    public shorts: boolean;
    public distributions: boolean;

    constructor(private service: TradingService, private dateService: DateService) {
    }

    ngOnInit() {
        this.ranking();
    }

    ranking(datarif?: Date) {
        this.loading = true;

        if (datarif == null) {
            datarif = this.dateService.addMonths(new Date(), 1);
        }
        this.service.ranking(datarif).then(result => {
            this.etfs = result;
            this.dataRif = datarif;
            this.dataPrecedente = this.dateService.addMonths(datarif, -1);
            this.dataSuccessiva = this.dateService.addMonths(datarif, 1);
            this.tabellaTraderLinkUrl = this.service.getTabellaTraderLinkUrl(result);
            this.loading = false;
        });
    }
    meseAnnoPrecedente() {
        if (this.dataPrecedente != null) {
            return this.dateService.getMeseAnnoString(this.dataPrecedente);
        }
    }
    meseAnnoSuccessivo() {
        if (this.dataSuccessiva != null)
            return this.dateService.getMeseAnnoString(this.dataSuccessiva);
    }
    meseAnnoCorrente() {
        if (this.dataRif != null)
            return this.dateService.getMeseAnnoString(this.dataRif);
    }
    aggiornaQuote() {
        this.loading = true;
        this.service.updateQuotes().then(result => {
            this.ranking(this.dataRif);
            this.quoteAggiornate = ' (' + result + ')';
        });
    }
    aggiornaQuoteMeseSuccessivo() {
        this.loading = true;
        this.service.updateQuotesNextMonth().then(result => {
            this.ranking(this.dataRif);
            this.quoteAggiornateMeseSuccessivo = ' (' + result + ')';
        });
    }
    removeFromSelection(etf) {
        this.service.removeFromSelection(etf.ticker).then(result => {
            this.ranking();
        });
    }
}
