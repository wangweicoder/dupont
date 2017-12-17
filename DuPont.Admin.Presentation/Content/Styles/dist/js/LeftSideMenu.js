
var curMenu = null, zTree_Menu = null;
var setting = {
    view: {
        showLine: true,
        selectedMulti: false,
        dblClickExpand: false
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onNodeCreated: this.onNodeCreated,
        beforeClick: this.beforeClick,
        onClick: this.onClick
    }
};

var zNodes =[
    { id:1, pId:0, name:"主菜单 1", open:true},
    { id:11, pId:1, name:"子菜单 1-1"},
    { id:111, pId:11, name:"叶子节点 1-1-1"},
    { id:112, pId:11, name:"叶子节点 1-1-2"},
    { id:113, pId:11, name:"叶子节点 1-1-3"},
    { id:114, pId:11, name:"叶子节点 1-1-4"},
    { id:12, pId:1, name:"子菜单 1-2"},
    { id:121, pId:12, name:"叶子节点 1-2-1"},
    { id:122, pId:12, name:"叶子节点 1-2-2"},
    { id:123, pId:12, name:"叶子节点 1-2-3"},
    { id:124, pId:12, name:"叶子节点 1-2-4"},
    { id:2, pId:0, name:"主菜单 2"},
    { id:21, pId:2, name:"子菜单 2-1"},
    { id:211, pId:21, name:"叶子节点 2-1-1"},
    { id:212, pId:21, name:"叶子节点 2-1-2"},
    { id:213, pId:21, name:"叶子节点 2-1-3"},
    { id:214, pId:21, name:"叶子节点 2-1-4"},
    { id:22, pId:2, name:"子菜单 2-2"},
    { id:221, pId:22, name:"叶子节点 2-2-1"},
    { id:222, pId:22, name:"叶子节点 2-2-2"},
    { id:223, pId:22, name:"叶子节点 2-2-3"},
    { id:224, pId:22, name:"叶子节点 2-2-4"},
    { id:3, pId:0, name:"主菜单 3"},
    { id:31, pId:3, name:"子菜单 3-1"},
    { id:311, pId:31, name:"叶子节点 3-1-1"},
    { id:312, pId:31, name:"叶子节点 3-1-2"},
    { id:313, pId:31, name:"叶子节点 3-1-3"},
    { id:314, pId:31, name:"叶子节点 3-1-4"},
    { id:32, pId:3, name:"子菜单 3-2"},
    { id:321, pId:32, name:"叶子节点 3-2-1"},
    { id:322, pId:32, name:"叶子节点 3-2-2"},
    { id:323, pId:32, name:"叶子节点 3-2-3"},
    { id:324, pId:32, name:"叶子节点 3-2-4"}			
];

function beforeClick(treeId, node) {
    if (node.isParent) {
        if (node.level === 0) {
            var pNode = curMenu;
            while (pNode && pNode.level !==0) {
                pNode = pNode.getParentNode();
            }
            if (pNode !== node) {
                var a = $("#" + pNode.tId + "_a");
                a.removeClass("cur");
                zTree_Menu.expandNode(pNode, false);
            }
            a = $("#" + node.tId + "_a");
            a.addClass("cur");

            var isOpen = false;
            for (var i=0,l=node.children.length; i<l; i++) {
                if(node.children[i].open) {
                    isOpen = true;
                    break;
                }
            }
            if (isOpen) {
                zTree_Menu.expandNode(node, true);
                curMenu = node;
            } else {
                zTree_Menu.expandNode(node.children[0].isParent?node.children[0]:node, true);
                curMenu = node.children[0];
            }
        } else {
            zTree_Menu.expandNode(node);
        }
    }
    return !node.isParent;
}
function onClick(e, treeId, node) {
    alert("Do what you want to do!");
}

$(document).ready(function(){
    $.fn.zTree.init($("#leftSideMenu"), setting, zNodes);
    zTree_Menu = $.fn.zTree.getZTreeObj("leftSideMenu");
    curMenu = zTree_Menu.getNodes()[0].children[0].children[0];
    zTree_Menu.selectNode(curMenu);
    var a = $("#" + zTree_Menu.getNodes()[0].tId + "_a");
    a.addClass("cur");
});
