import { Component, EventEmitter, Input, Output} from '@angular/core';
import { Portfolio } from '../models/portfolio';
import { TradingService } from '../services/trading.service';

@Component({
    selector: 'edit-bilancio',
    templateUrl: './edit-bilancio.component.html',
    styles: ['.modal { background: rgba(0,0,0,0.6); }']
})
export class EditBilancioComponent{
    @Input() portfolio: Portfolio;
    @Output() afterSave: EventEmitter<any> = new EventEmitter();

    public visible = false;
    private visibleAnimate = false;

    constructor(private service: TradingService) {
    }

    public show(): void {
        this.visible = true;
        setTimeout(() => this.visibleAnimate = true);
    }

    public hide(): boolean {
        this.visibleAnimate = false;
        setTimeout(() => this.visible = false, 300);
        return false;
    }

    save() {
        let bilancio = {
            data: this.portfolio.dataBilancio,
            invested: this.portfolio.totaleInvestito,
            cash: this.portfolio.cash,
            minusvalenze: this.portfolio.minusvalenze
        };

        this.service.saveBilancio(bilancio).then(result => {
            this.hide();
            this.afterSave.emit();
        });
    }
}
