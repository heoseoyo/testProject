 using System.Collections.Generic;
using static UnityUtility;
using static WidgetUtility;
using static FrameUtility;
using static StringUtility;
using System;

// 测试衣服对象的示例
public class ClothObject : ClassObject
{ 


    public string mName;
    public override void resetProperty()
    {
        base.resetProperty();
        mIcon = null;
        mName = null;
    }
}

public class ClothesItem : WindowRecycleableUGUI
{
    protected myUGUIImage mIcon;
    protected myUGUIText mItemName;
    protected ClothObject mCloth;
    public ClothesItem(LayoutScript script) : base(script) { }
    public override void assignWindow(myUIObject parent, myUIObject template, string name)
    {
        base.assignWindow(parent, template, name);
        newObject(out mIcon, "Icon");
        newObject(out mItemName, "ItemName");
    }
    public override void init()
    {
        base.init();
        mRoot.registeCollider(onItemClick);
    }
    public void setCloth(ClothObject cloth)
    {
        mCloth = cloth;
        mIcon.setActive(mCloth != null);
        mItemName.setActive(mCloth != null);
        if (mCloth == null)
        {
            return;
        }
        mIcon.setSpriteName(mCloth.mIcon);
        mItemName.setText(mCloth.mName);
    }
    public ClothObject getCloth() { return mCloth; }
    //---------------------------------------------------------------------------------------------------------------------------
    protected void onItemClick()
    {
        log("点击衣服");
    }
}

public class UIClothesPanel : LayoutScript
{
    protected WindowStructPool<ClothesItem> mClothesPool;
    protected myUGUIObject mClothesPanel;
    protected myUGUIObject mCategoryPanel;
    protected myUGUIObject mItemViewport;
    protected myUGUIDragView mClothesContent;
    protected myUGUIObject mClothesTemplate;
    protected myUGUIObject mCloseButton;
    public UIClothesPanel()
    {
        mClothesPool = new(this);
    }
    public override void assignWindow()
    {
        newObject(out mClothesPanel, "ClothesPanel");
        newObject(out mCategoryPanel, mClothesPanel, "CategoryPanel");
        newObject(out mItemViewport, mClothesPanel, "ItemViewport");
        newObject(out mClothesContent, mItemViewport, "ClothesContent");
        newObject(out mClothesTemplate, mClothesContent, "ClothesTemplate", 0);
        newObject(out mCloseButton, "ButtonReturn");
    }
    public override void init()
    {
        base.init();
        mClothesPool.init(mClothesContent, mClothesTemplate);
        mClothesContent.initDragView(mItemViewport, DRAG_DIRECTION.VERTICAL);
        mCloseButton.registeCollider(onCloseClick);
    }

    protected void onCloseClick()
    {
        LT.HIDE<UIClothesPanel>();
    }

    public override void onGameState()
    {
        base.onGameState();
        using var a = new ListScope<ClothObject>(out var clothList);
        for (int i = 0; i < 50; ++i)
        {
            CLASS(out ClothObject cloth);
            cloth.mIcon = "Icon" + IToS(i);
            cloth.mName = "Cloth" + IToS(i);
            clothList.add(cloth);
        }
        setClothList(clothList);
    }
    public void setClothList(List<ClothObject> clothList)
    {
        mClothesPool.unuseAll();
        foreach (ClothObject item in clothList)
        {
            mClothesPool.newItem().setCloth(item);
        }
        autoGridFixedRootWidth(mClothesContent, mClothesTemplate.getWindowSize());
    }
}