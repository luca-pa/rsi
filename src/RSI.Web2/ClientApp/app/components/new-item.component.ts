import { Component, EventEmitter, Input, Output} from '@angular/core';
import { PortfolioItem } from '../models/portfolio-item';
import { TradingService } from '../services/trading.service';

@Component({
    selector: 'new-item',
    templateUrl: './new-item.component.html',
    styles: ['.modal { background: rgba(0,0,0,0.6); }']
})
export class NewItemComponent{
    item: PortfolioItem = new PortfolioItem();
    @Output() afterSave: EventEmitter<any> = new EventEmitter();

    public visible = false;
    private visibleAnimate = false;

    constructor(private service: TradingService) {
    }

    private getNewItem()
    {
        return new PortfolioItem ({
            data: this.service.getDataLocaleString(new Date()),
            ticker: '',
            prezzoAcquisto: '',
            quantita: ''
        });
    }

    public show(): void {
        this.item = this.getNewItem();
        this.visible = true;
        setTimeout(() => this.visibleAnimate = true);
    }

    public hide(): boolean {
        this.visibleAnimate = false;
        setTimeout(() => this.visible = false, 300);
        return false;
    }

    save() {
        this.service.savePortfolioItem(this.item).then(result => {
            this.hide();
            this.afterSave.emit();
        });
    }
}
