/*
This demo visualises use case and test case and source code pairings.
*/

$(function () {

    var lastSearch = '';
    var layoutPadding = 20;
    var aniDur = 500;
    var easing = 'linear';
    var cy;
    var allNodes = null;
    var allEles = null;
    var lastHighlighted = null;
    var lastUnhighlighted = null;

    var infoTemplate = Handlebars.compile([
        '<p class="ac-name">{{name}}</p>',
        '<p class="ac-node-type"><i class="fa fa-info-circle"></i> {{NodeTypeFormatted}} {{#if Type}}({{Type}}){{/if}}</p>',
        '<p class="ac-more"><i class="fa fa-external-link"></i> {{#if FileUrl}}{{ link "More Information" FileUrl }}{{/if}}</p>',
        '{{#if functionhtmlformatted}}<p class="ac-function-head">List of Class Functions</p><p class="ac-function">{{functionhtmlformatted}}</p>{{/if}}'
    ].join(''));

    Handlebars.registerHelper('link', function (text, url) {
        url = Handlebars.escapeExpression(url);
        text = Handlebars.escapeExpression(text);
        var url_dec = decodeURIComponent(url).replace(/\+/g, " ");
        return new Handlebars.SafeString("<a href='" + url_dec + "'>" + text + "</a>");
    });

    function getFadePromise(ele, opacity) {
        return ele.animation({
            style: { 'opacity': opacity },
            duration: aniDur
        }).play().promise();
    };

    var restoreElesPositions = function (nhood) {
        return Promise.all(nhood.map(function (ele) {
            var p = ele.data('orgPos');

            return ele.animation({
                position: { x: p.x, y: p.y },
                duration: aniDur,
                easing: easing
            }).play().promise();
        }));
    };

    function highlight(node) {
        var oldNhood = lastHighlighted;

        var nhood = lastHighlighted = node.closedNeighborhood();
        var others = lastUnhighlighted = cy.elements().not(nhood);

        var reset = function () {
            cy.batch(function () {
                others.addClass('hidden');
                nhood.removeClass('hidden');

                allEles.removeClass('faded highlighted');

                nhood.addClass('highlighted');

                others.nodes().forEach(function (n) {
                    var p = n.data('orgPos');

                    n.position({ x: p.x, y: p.y });
                });
            });

            return Promise.resolve().then(function () {
                if (isDirty()) {
                    return fit();
                } else {
                    return Promise.resolve();
                };
            }).then(function () {
                return Promise.delay(aniDur);
            });
        };

        var runLayout = function () {
            var p = node.data('orgPos');

            var l = nhood.filter(':visible').makeLayout({
                name: 'concentric',
                fit: false,
                animate: true,
                animationDuration: aniDur,
                animationEasing: easing,
                boundingBox: {
                    x1: p.x - 1,
                    x2: p.x + 1,
                    y1: p.y - 1,
                    y2: p.y + 1
                },
                avoidOverlap: true,
                concentric: function (ele) {
                    if (ele.same(node)) {
                        return 2;
                    } else {
                        return 1;
                    }
                },
                levelWidth: function () { return 1; },
                padding: layoutPadding
            });

            var promise = cy.promiseOn('layoutstop');

            l.run();

            return promise;
        };

        var fit = function () {
            return cy.animation({
                fit: {
                    eles: nhood.filter(':visible'),
                    padding: layoutPadding
                },
                easing: easing,
                duration: aniDur
            }).play().promise();
        };

        var showOthersFaded = function () {
            return Promise.delay(250).then(function () {
                cy.batch(function () {
                    others.removeClass('hidden').addClass('faded');
                });
            });
        };

        return Promise.resolve()
            .then(reset)
            .then(runLayout)
            .then(fit)
            .then(showOthersFaded)
            ;

    }

    function isDirty() {
        return lastHighlighted != null;
    }

    function clear(opts) {
        if (!isDirty()) { return Promise.resolve(); }

        opts = $.extend({

        }, opts);

        cy.stop();
        allNodes.stop();

        var nhood = lastHighlighted;
        var others = lastUnhighlighted;

        lastHighlighted = lastUnhighlighted = null;

        var hideOthers = function () {
            return Promise.delay(125).then(function () {
                others.addClass('hidden');

                return Promise.delay(125);
            });
        };

        var showOthers = function () {
            cy.batch(function () {
                allEles.removeClass('hidden').removeClass('faded');
            });

            return Promise.delay(aniDur);
        };

        var restorePositions = function () {
            cy.batch(function () {
                others.nodes().forEach(function (n) {
                    var p = n.data('orgPos');

                    n.position({ x: p.x, y: p.y });
                });
            });

            return restoreElesPositions(nhood.nodes());
        };

        var resetHighlight = function () {
            nhood.removeClass('highlighted');
        };

        return Promise.resolve()
            .then(resetHighlight)
            .then(hideOthers)
            .then(restorePositions)
            .then(showOthers)
            ;
    }

    function showNodeInfo(node) {
        $('#info').html(infoTemplate(node.data())).show();
    }

    function hideNodeInfo() {
        $('#info').hide();
    }

    function initCy(then) {
        var loading = document.getElementById('divloading');
        var expJson = then[0];
        var styleJson = then[1];
        var elements = expJson.Data.elements;

        elements.nodes.forEach(function (n) {
            var data = n.data;

            data.NodeTypeFormatted = data.NodeType;

            if (data.NodeTypeFormatted === 'UseCase') {
                data.NodeTypeFormatted = 'Use Case';
            } else if (data.NodeTypeFormatted === 'TestCase') {
                data.NodeTypeFormatted = 'Test Case';
            }
            else if (data.NodeTypeFormatted === 'SourceCode') {
                data.NodeTypeFormatted = 'Source Code';
            }

            n.data.orgPos = {
                x: n.position.x,
                y: n.position.y
            };

        });

        loading.classList.add('loaded');

        cy = window.cy = cytoscape({
            container: $('#cy'),
            layout: { name: 'preset', padding: layoutPadding },
            style: styleJson,
            elements: elements,
            motionBlur: true,
            selectionType: 'single',
            boxSelectionEnabled: false,
            autoungrabify: true
        });

        allNodes = cy.nodes();
        allEles = cy.elements();

        cy.on('free', 'node', function (e) {
            var n = e.cyTarget;
            var p = n.position();

            n.data('orgPos', {
                x: p.x,
                y: p.y
            });
        });

        cy.on('tap', function () {
            $('#search').blur();
        });

        cy.on('select unselect', 'node', _.debounce(function (e) {
            var node = cy.$('node:selected');

            if (node.nonempty()) {
                showNodeInfo(node);

                Promise.resolve().then(function () {
                    return highlight(node);
                });
            } else {
                hideNodeInfo();
                clear();
            }

        }, 100));

    }

    function loadData() {
        $("#divloading").show();
        $("#cy").hide();
        var graphP = $.ajax({
            url: '/api/IR/GetData',
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify({
                "MissingLinks": $("#chkmissinglinks").is(':checked'),
                "ExcludeTestCase": $("#chktestcase").is(':checked'),
                "ExcludeSourceCode": $("#chksourcecode").is(':checked'),
                "SimilarityScore": $('#rngsimilarityscore').val(),
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                return data;
            },
            failure: function (errorMsg) {
                alert("Error retrieving message : " + errorMsg);
            }
        });

        // also get style via ajax
        var styleP = $.ajax({
            url: '/Scripts/style.cycss',
            type: 'GET',
            dataType: 'text'
        }); 
        // when both graph export json and style loaded, init cy
        Promise.all([graphP, styleP]).then(initCy);
        $("#divloading").hide();
        $("#cy").show();
    }
       
    $('#search').typeahead({
        minLength: 2,
        highlight: true,
    },
        {
            name: 'search-dataset',
            source: function (query, cb) {
                function matches(str, q) {
                    str = (str || '').toLowerCase();
                    q = (q || '').toLowerCase();

                    return str.match(q);
                }

                var fields = ['name', 'NodeType'];
                function anyFieldMatches(n) {
                    for (var i = 0; i < fields.length; i++) {
                        var f = fields[i];

                        if (matches(n.data(f), query)) {
                            return true;
                        }
                    }

                    return false;
                }

                function getData(n) {
                    var data = n.data();

                    return data;
                }

                function sortByName(n1, n2) {
                    if (n1.data('name') < n2.data('name')) {
                        return -1;
                    } else if (n1.data('name') > n2.data('name')) {
                        return 1;
                    }

                    return 0;
                }

                var res = allNodes.stdFilter(anyFieldMatches).sort(sortByName).map(getData);

                cb(res);
            },
            templates: {
                suggestion: infoTemplate
            }
        }).on('typeahead:selected', function (e, entry, dataset) {
            var n = cy.getElementById(entry.id);

            cy.batch(function () {
                allNodes.unselect();
                n.select();
            });

            showNodeInfo(n);
        }).on('keydown keypress keyup change', _.debounce(function (e) {
            var thisSearch = $('#search').val();

            if (thisSearch !== lastSearch) {
                $('.tt-dropdown-menu').scrollTop(0);

                lastSearch = thisSearch;
            }
        }, 50));

    $('#reset').on('click', function () {
        if (isDirty()) {
            clear();
        } else {
            allNodes.unselect();
            hideNodeInfo();
            cy.stop();

            cy.animation({
                fit: {
                    eles: cy.elements(),
                    padding: layoutPadding
                },
                duration: aniDur,
                easing: easing
            }).play();
        }
    });

    $('#btnFilter').on('click', function () {
        loadData();
    });

    $('.slider').on('input change', function () {
        $(this).next($('.slider_label')).html(this.value);
    });
    $('.slider_label').each(function () {
        var value = $(this).prev().attr('value');
        $(this).html(value);
    });  

/*Load Data*/
    $('[data-toggle="tooltip"]').tooltip(); 
    loadData();
});
