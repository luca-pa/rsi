import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import { Portfolio } from '../models/portfolio';
import { PortfolioItem } from '../models/portfolio-item';
import { Etf } from '../models/etf';

@Injectable()
export class TradingService {
    constructor(private http: Http) { }

    ranking(datarif: Date): Promise<Etf[]> {
        return this.http.get('api/ranking/' + this.getDataString(datarif))
            .toPromise()
            .then(response => response.json() as Etf[]);
    }

    all(datarif: Date, shorts: boolean, distributions: boolean): Promise<Etf[]> {
        return this.http.get('api/allranking/' + this.getDataString(datarif) + '/' + shorts + '/' + distributions)
            .toPromise()
            .then(response => response.json() as Etf[]);
    }

    portfolio(): Promise<Portfolio> {
        return this.http.get('api/portfolio/')
            .toPromise()
            .then(response => response.json() as Portfolio);
    }

    portfolioPerformance() {
        return this.http.get('api/portfolio/true').toPromise();
    }

    updatePortfolioQuotes() {
        return this.http.get('api/quotes/false/true')
            .toPromise()
            .then(response => response.json() as number);
    }

    savePortfolioItem(item) {
        return this.http.post('api/portfolio/', JSON.stringify(item), this.options()).toPromise();
    }

    saveBilancio(bilancio) {
        return this.http.post('api/bilancio', JSON.stringify(bilancio), this.options()).toPromise();
    }

    charts() {
        return this.http.get('api/charts/').toPromise();
    }

    updateQuotes() {
        return this.http.get('api/quotes')
            .toPromise()
            .then(response => response.json() as number);
    }

    updateQuotesNextMonth() {
        return this.http.get('api/quotes/true')
            .toPromise()
            .then(response => response.json() as number);
    }

    removeFromSelection(ticker) {
        return this.http.delete('api/ranking/' + ticker)
            .toPromise();
    }

    addToSelection(ticker) {
        return this.http.put('api/ranking/' + ticker, '').toPromise();
    }

    private options() {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }

    getDataString(datarif?: Date): string {
        if (datarif !== null) {
            var month = datarif.getMonth() + 1;
            var year = datarif.getFullYear();
            return year + '-' + month + '-1';
        }
        return '';
    }

    getDataLocaleString(datarif) {
        if (datarif !== null) {
            var day = datarif.getDate();
            if (day < 10)
                day = '0' + day;
            var month = datarif.getMonth() + 1;
            if (month < 10)
                month = '0' + month;
            var year = datarif.getFullYear();
            return day + '/' + month + '/' + year;
        }
        return '';
    }

    getTabellaTraderLinkUrl(items): string {
        var url = "http://213.92.13.40/phprealtime/index.php?modo=t&lang=it&appear=n";

        for (var i = 0; i < items.length; i++) {
            url += "&id" + (i + 1) + "=" + items[i].isin;
        }
        return url + "&u=43705&p=Z58H50";
    }
}
