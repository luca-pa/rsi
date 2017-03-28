import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Etf } from '../models/etf';
import { TradingService } from '../services/trading.service';
import { DateService } from '../services/date.service';

@Component({
    selector: 'all',
    templateUrl: './ranking.component.html',
    styleUrls: ['./ranking.component.css']
})
export class AllRankingComponent implements OnInit {
    etfs: Etf[];
    loading: boolean = true;
    selection: boolean = false;
    all: boolean = true;
    dataRif: Date;
    dataPrecedente: Date;
    dataSuccessiva: Date;
    quoteAggiornate: string;
    quoteAggiornateMeseSuccessivo: string;
    shorts: boolean;
    distributions: boolean;

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
        this.service.all(datarif, this.shorts, this.distributions).then(result => {
            this.etfs = result;
            this.dataRif = datarif;
            this.dataPrecedente = this.dateService.addMonths(datarif, -1);
            this.dataSuccessiva = this.dateService.addMonths(datarif, 1);
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
    addToSelection(etf) {
        this.service.addToSelection(etf.ticker).then(result => {
        });
    };
}
