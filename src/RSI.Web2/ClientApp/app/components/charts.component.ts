import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Etf } from '../models/etf';
import { TradingService } from '../services/trading.service';
import * as _ from 'underscore';
declare var c3;

@Component({
    selector: 'charts',
    templateUrl: './charts.component.html',
    styleUrls: ['./charts.component.css']
})
export class ChartsComponent implements OnInit {
    public etfs: Etf[];
    public loading: boolean = true;

    constructor(private service: TradingService) {
    }

    ngOnInit() {
        this.service.charts().then(result => {
            this.etfs = result.json();

            setTimeout(() => this.renderCharts(), 1000);
        });
    }

    renderCharts() {
        for (var i = 0; i < this.etfs.length; i++) {
            this.renderChart(this.etfs[i]);
        }
        this.loading = false;
    }

    renderChart(etf) {

        var times = _.union(['x'], etf.times);
        var smas = _.union(['sma'], etf.smas);
        var values = _.union([etf.ticker], etf.values);

        c3.generate({
            bindto: '#chart-' + etf.ticker,
            data: {
                x: 'x',
                columns: [times, smas, values]
            },
            axis: {
                x: {
                    type: 'timeseries',
                    tick: {
                        format: '%m-%Y'
                    }
                }
            }

        });

    }
}
