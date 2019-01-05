// HomographyMakeInCpp.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include "pch.h"
#include <iostream>

using namespace cv;
using std::vector;

void test1()
{
    vector<Point> srcVec(4);
    vector<Point> dstVec(4);

    Point s0(0, 0);
    Point s1(100, 0);
    Point s2(100, 100);
    Point s3(0, 100);

    srcVec.push_back(s0);
    srcVec.push_back(s1);
    srcVec.push_back(s2);
    srcVec.push_back(s3);

    Point d0(0, 0);
    Point d1(500, 0);
    Point d2(500, 200);
    Point d3(0, 200);

    dstVec.push_back(d0);
    dstVec.push_back(d1);
    dstVec.push_back(d2);
    dstVec.push_back(d3);

    Mat homographyMat = findHomography(srcVec, dstVec, 0);
    std::cout << "=====test1=====" << std::endl;
    std::cout << homographyMat << std::endl;
}


void test2()
{
    vector<Point> srcVec(4);
    vector<Point> dstVec(4);

    Point s0(123, 541);
    Point s1(362, 794);
    Point s2(362, -300);
    Point s3(123, -203);

    srcVec.push_back(s0);
    srcVec.push_back(s1);
    srcVec.push_back(s2);
    srcVec.push_back(s3);

    Point d0(60, 777);
    Point d1(1320, 444);
    Point d2(1423, -5041);
    Point d3(-200, 609);

    dstVec.push_back(d0);
    dstVec.push_back(d1);
    dstVec.push_back(d2);
    dstVec.push_back(d3);

    Mat homographyMat = findHomography(srcVec, dstVec, 0);
    std::cout << "=====test2=====" << std::endl;
    std::cout << homographyMat << std::endl;
}


int main()
{
    test1();
    test2();
}

// プログラムの実行: Ctrl + F5 または [デバッグ] > [デバッグなしで開始] メニュー
// プログラムのデバッグ: F5 または [デバッグ] > [デバッグの開始] メニュー

// 作業を開始するためのヒント: 
//    1. ソリューション エクスプローラー ウィンドウを使用してファイルを追加/管理します 
//   2. チーム エクスプローラー ウィンドウを使用してソース管理に接続します
//   3. 出力ウィンドウを使用して、ビルド出力とその他のメッセージを表示します
//   4. エラー一覧ウィンドウを使用してエラーを表示します
//   5. [プロジェクト] > [新しい項目の追加] と移動して新しいコード ファイルを作成するか、[プロジェクト] > [既存の項目の追加] と移動して既存のコード ファイルをプロジェクトに追加します
//   6. 後ほどこのプロジェクトを再び開く場合、[ファイル] > [開く] > [プロジェクト] と移動して .sln ファイルを選択します
