import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PortfolioItem } from '../models/portfolio-item';
import { TradingService } from '../services/trading.service';

@Component({
    selector: 'edit-item',
    templateUrl: './edit-item.component.html'
})
export class EditItemComponent {
    @Input() item: PortfolioItem;
    @Output() afterSave = new EventEmitter();

    constructor(private service: TradingService) {
    }

    save() {
        this.service.savePortfolioItem(this.item).then(result => {
            this.afterSave.emit();
        });
    }
}
