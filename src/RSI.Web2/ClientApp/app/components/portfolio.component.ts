import { Component, OnInit, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { Http } from '@angular/http';

import { EditItemComponent } from './edit-item.component';
import { NewItemComponent } from './new-item.component';
import { EditBilancioComponent } from './edit-bilancio.component';

import { Portfolio } from '../models/portfolio';
import { PortfolioItem } from '../models/portfolio-item';
import { TradingService } from '../services/trading.service';
declare var c3;

@Component({
    selector: 'portfolio',
    templateUrl: './portfolio.component.html',
    styleUrls: ['./portfolio.component.css']
})
export class PortfolioComponent implements OnInit {
    @ViewChildren(EditItemComponent) editItemComponents: QueryList<EditItemComponent>;
    @ViewChild(NewItemComponent) newItemComponent: NewItemComponent;
    @ViewChild(EditBilancioComponent) editBilancioComponent: EditBilancioComponent;

    public portfolio: Portfolio;
    public loading: boolean;
    public loadingChart: boolean;
    public tabellaTraderLinkUrl: string;
    public quoteAggiornate: string;

    constructor(private service: TradingService) {
    }

    ngOnInit() {
        this.load();
    }

    load() {
        this.loading = true;

        this.service.portfolio().then(result => {
            this.portfolio = result;
            this.tabellaTraderLinkUrl = this.service.getTabellaTraderLinkUrl(result.items);
            this.loading = false;
        })

    }
    showEditItem(ticker: string) {
        this.editItemComponents.find(i => i.item.ticker == ticker).show();
    }
    showNewItem() {
        this.newItemComponent.show();
    }
    showEditBilancio() {
        this.editBilancioComponent.show();
    }

    updatePortfolioQuotes() {
        this.loadingChart = true;

        this.service.updatePortfolioQuotes().then(result => {
            this.quoteAggiornate = ' (' + result + ')';
            this.showChart();
        })
    };

    showChart() {
        this.loadingChart = true;
        setTimeout(() => this.renderChart(), 200);
    };

    renderChart() {

        this.service.portfolioPerformance().then(result => {

            c3.generate({
                bindto: '#performance-chart',
                data: {
                    json: result.json(),
                    keys: {
                        value: ['data', 'value']
                    },
                    x: 'data',
                    names: {
                        value: 'TR %'
                    }
                },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: {
                            //format: '%Y-%m-%d'
                            format: '%d-%m-%Y'
                        }
                    }
                },
                grid: {
                    y: {
                        lines: [
                            { value: 0 }
                        ]
                    }
                },
                point: {
                    r: 2
                }
            });

            setTimeout(() => this.loadingChart = false, 200)
        });
    };

}
