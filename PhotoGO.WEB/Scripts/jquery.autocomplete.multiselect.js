// http://jsfiddle.net/mekwall/sgxKJ/

$.widget("ui.autocomplete", $.ui.autocomplete, {
    options : $.extend({}, this.options, {
        multiselect: false
    }),
    _create: function(){
        this._super();

        $("#myAutocomplete").css({ "height": 32, "margin-left": 5 });

        var self = this,
            o = self.options;

        if (o.multiselect) {
            self.selectedItems = {};           
            self.multiselect = $("<div></div>")
                .addClass("ui-autocomplete-multiselect ui-state-default ui-widget")
                .css({ "height": 34, "width": 230, "cursor": "text", "padding-left": 5, "overflow-y": "hidden"})
                .insertBefore(self.element)
                .append(self.element)
                .bind("click.autocomplete", function(){
                    //self.element.focus();
                    document.getElementById("myAutocomplete").focus();
                });
            
            var fontSize = parseInt(self.element.css("fontSize"), 12);
            function autoSize(e){
                // Hackish autosizing
                var $this = $(this);
                $this.width(1).width(this.scrollWidth+fontSize-1);
            };

            var kc = $.ui.keyCode;
            self.element.bind({
                "keydown.autocomplete": function(e){
                    if ((this.value === "") && (e.keyCode == kc.BACKSPACE)) {
                        var prev = self.element.prev();
                        delete self.selectedItems[prev.text()];
                        prev.remove();
                    }
                },
                "focus.autocomplete blur.autocomplete": function(){
                    self.multiselect.toggleClass("ui-state-active");
                },
                "keypress.autocomplete change.autocomplete focus.autocomplete blur.autocomplete": autoSize
            }).trigger("change");

            o.select = o.select || function(e, ui) {
                $("<div></div>")
                    .addClass("ui-autocomplete-multiselect-item")
                    .text(ui.item.label)
                    .append(
                        $("<span></span>")
                        .addClass("ui-icon ui-icon-close")
                            .click(function(){
                                var item = $(this).parent();
                                delete self.selectedItems[item.text()];
                                item.remove();
                            })
                    )
                    .insertBefore(self.element);
                
                self.selectedItems[ui.item.label] = ui.item;
                self._value("");
                return false;
            }

            /*self.options.open = function(e, ui) {
                var pos = self.multiselect.position();
                pos.top += self.multiselect.height();
                self.menu.element.position(pos);
            }*/
        }

        return this;
    }
});